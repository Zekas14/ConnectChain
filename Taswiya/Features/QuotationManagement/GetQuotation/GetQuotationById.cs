using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Quotation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.QuotationManagement.GetQuotation
{
    public record GetQuotationByIdQuery(int QuotationId) : IRequest<RequestResult<GetQuotationViewModel>>;


    public class GetQuotationByIdQueryHandler : IRequestHandler<GetQuotationByIdQuery, RequestResult<GetQuotationViewModel>>
    {
        private readonly IRepository<Quotation> _quotationRepository;

        public GetQuotationByIdQueryHandler(IRepository<Quotation> quotationRepository)
        {
            _quotationRepository = quotationRepository;
        }

        public async Task<RequestResult<GetQuotationViewModel>> Handle(GetQuotationByIdQuery request, CancellationToken cancellationToken)
        {
            var quotation = _quotationRepository.GetAllWithIncludes(q => q
                .Where(x => x.ID == request.QuotationId)
                .Include(x => x.Supplier)
            ).FirstOrDefault();

            if (quotation == null)
                return RequestResult<GetQuotationViewModel>.Failure(ErrorCode.NotFound, "Quotation not found.");

            var viewModel = new GetQuotationViewModel
            {
                Id = quotation.ID,
                RfqId = quotation.RfqId,
                SupplierId = quotation.SupplierId,
                SupplierName = quotation.Supplier?.Name ?? "",
                UnitPrice = quotation.UnitPrice,
                ProductId = quotation.ProductId,
                Quantity = quotation.Quantity,
                PaymentTermId = quotation.PaymentTermId,
                DeliveryTerm = quotation.DeliveryTerm,
                DeliveryFee = quotation.DeliveryFee,
                DeliveryTimeInDays = quotation.DeliveryTimeInDays,
                Notes = quotation.Notes,
                ValidUntil = quotation.ValidUntil,
                CreatedAt = quotation.CreatedDate
            };

            return RequestResult<GetQuotationViewModel>.Success(viewModel, "Quotation fetched successfully.");
        }
    }

}
