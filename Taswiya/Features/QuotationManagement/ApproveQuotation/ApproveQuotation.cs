using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.QuotationManagement.ApproveQuotation.Event;
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
        private readonly IMediator _mediator;
        public ApproveQuotationCommandHandler(
            IRepository<Quotation> quotationRepository,
            IRepository<RFQ> rfqRepository,
            IMediator mediator)
        {
            _quotationRepository = quotationRepository;
            _rfqRepository = rfqRepository;
            _mediator = mediator;
        }

        public async Task<RequestResult<bool>> Handle(ApproveQuotationCommand request, CancellationToken cancellationToken)
        {
            var quotation = _quotationRepository.GetAllWithIncludes(q => q
                .AsTracking()
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
                    
                    _quotationRepository.Update(q);
                   await _mediator.Publish(new ApproveQuotationEvent(request.QuotationId),cancellationToken);
                   
                }
                else if (q.Status != Models.Enums.QuotationStatus.Rejected)
                {
                    q.Status = Models.Enums.QuotationStatus.Rejected;
                    _quotationRepository.Update(q);
                }

               
            }
            await _quotationRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Quotation approved successfully.");
        }
    }



}
