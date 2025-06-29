using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Quotation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.QuotationManagement.GetQuotation
{
    public record GetQuotationsByRFQQuery(int RfqId) : IRequest<RequestResult<List<GetQuotationViewModel>>>;



    public class GetQuotationsByRFQQueryHandler : IRequestHandler<GetQuotationsByRFQQuery, RequestResult<List<GetQuotationViewModel>>>
    {
        private readonly IRepository<Quotation> _quotationRepository;

        public GetQuotationsByRFQQueryHandler(IRepository<Quotation> quotationRepository)
        {
            _quotationRepository = quotationRepository;
        }

        public async Task<RequestResult<List<GetQuotationViewModel>>> Handle(GetQuotationsByRFQQuery request, CancellationToken cancellationToken)
        {
            var quotations = _quotationRepository.GetAllWithIncludes(q => q
                .Where(x => x.RfqId == request.RfqId)
                .Include(x => x.Supplier)
                .Include(x => x.Product)
            ).ToList();

            var result = quotations.Select(q => new GetQuotationViewModel
            {
                Id = q.ID,
                RfqId = q.RfqId,
                SupplierId = q.SupplierId,
                ProductId = q.ProductId,
                CategoryId = q.CategoryId,
                ProductName = q.Product?.Name ?? "",
                SupplierName = q.Supplier?.Name ?? "",
                Status = q.Status,
                Quantity = q.Quantity,
                UnitPrice = q.UnitPrice,
                PaymentTermId = q.PaymentTermId,
                DeliveryTerm = q.DeliveryTerm,
                DeliveryFee = q.DeliveryFee,
                DeliveryTimeInDays = q.DeliveryTimeInDays,
                Notes = q.Notes,
                ValidUntil = q.ValidUntil,
                CreatedAt = q.CreatedDate
            }).ToList();

            return RequestResult<List<GetQuotationViewModel>>.Success(result, "Quotations fetched successfully.");
        }
    }
}
