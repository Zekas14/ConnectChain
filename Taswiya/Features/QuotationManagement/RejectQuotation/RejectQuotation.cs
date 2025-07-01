using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.QuotationManagement.RejectQuotation
{
    public record RejectQuotationCommand(int QuotationId, string CustomerId) : IRequest<RequestResult<bool>>;

    public class RejectQuotationCommandHandler : IRequestHandler<RejectQuotationCommand, RequestResult<bool>>
    {
        private readonly IRepository<Quotation> _quotationRepository;

        public RejectQuotationCommandHandler(IRepository<Quotation> quotationRepository)
        {
            _quotationRepository = quotationRepository;
        }

        public async Task<RequestResult<bool>> Handle(RejectQuotationCommand request, CancellationToken cancellationToken)
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
                return RequestResult<bool>.Failure(ErrorCode.Forbidden, "You are not authorized to reject this quotation.");

            if (quotation.Status == Models.Enums.QuotationStatus.Rejected)
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "Quotation is already rejected.");

            quotation.Status = Models.Enums.QuotationStatus.Rejected;
            _quotationRepository.Update(quotation);
            await _quotationRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Quotation rejected successfully.");
        }
    }

}
