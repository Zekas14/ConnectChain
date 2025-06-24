using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.QuotationManagement.ApproveQuotation
{
    public record ApproveQuotationCommand(int QuotationId, string CustomerId) : IRequest<RequestResult<bool>>;

    public class ApproveQuotationCommandHandler : IRequestHandler<ApproveQuotationCommand, RequestResult<bool>>
    {
        private readonly IRepository<Quotation> _quotationRepository;
        private readonly IRepository<RFQ> _rfqRepository;

        public ApproveQuotationCommandHandler(
            IRepository<Quotation> quotationRepository,
            IRepository<RFQ> rfqRepository)
        {
            _quotationRepository = quotationRepository;
            _rfqRepository = rfqRepository;
        }

        public async Task<RequestResult<bool>> Handle(ApproveQuotationCommand request, CancellationToken cancellationToken)
        {
            var quotation = _quotationRepository.GetAllWithIncludes(q => q
                .Where(x => x.ID == request.QuotationId)
                .Include(x => x.RFQ)
            ).FirstOrDefault();

            if (quotation == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Quotation not found.");

            if (quotation.RFQ == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Associated RFQ not found.");

            if (quotation.RFQ.CustomerId != request.CustomerId)
                return RequestResult<bool>.Failure(ErrorCode.Forbidden, "You are not authorized to approve this quotation.");

            if (quotation.Status == Models.Enums.QuotationStatus.Approved)
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "Quotation is already approved.");

            
            var allQuotations = _quotationRepository.Get(q => q.RfqId == quotation.RfqId).ToList();
            foreach (var q in allQuotations)
            {
                if (q.ID == quotation.ID)
                {
                    q.Status = Models.Enums.QuotationStatus.Approved;
                }
                else if (q.Status != Models.Enums.QuotationStatus.Rejected)
                {
                    q.Status = Models.Enums.QuotationStatus.Rejected;
                }

               
            }
            await _quotationRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Quotation approved successfully.");
        }
    }



}
