using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.RFQ.GetRecommendedSuppliers;
using MediatR;

namespace ConnectChain.Features.RFQManagement.RecommendedSuppliers.Query
{
    public record RecommendSuppliersByProductCategoryQuery(int RfqId) : IRequest<RequestResult<List<RecommendedSupplierViewModel>>>;

    public class RecommendSuppliersByProductCategoryQueryHandler : IRequestHandler<RecommendSuppliersByProductCategoryQuery, RequestResult<List<RecommendedSupplierViewModel>>>
    {
        private readonly IRepository<RFQ> _rfqRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IRepository<Category> _categoryRepository;

        public RecommendSuppliersByProductCategoryQueryHandler(
            IRepository<RFQ> rfqRepository,
            IRepository<Supplier> supplierRepository,
            IRepository<Category> categoryRepository)
        {
            _rfqRepository = rfqRepository;
            _supplierRepository = supplierRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<RequestResult<List<RecommendedSupplierViewModel>>> Handle(RecommendSuppliersByProductCategoryQuery request, CancellationToken cancellationToken)
        {
            var rfq = _rfqRepository.GetAllWithIncludes(q => q.Where(x => x.ID == request.RfqId)
                .Select(x => new RFQ
                {
                    ID = x.ID,
                    ProductId = x.ProductId,
                    Product = x.Product,
                    Description = x.Description
                })).FirstOrDefault();

            if (rfq == null)
                return RequestResult<List<RecommendedSupplierViewModel>>.Failure(ErrorCode.NotFound, "RFQ not found.");

            int? categoryId = null;
            string categoryName = "";

            if (rfq.Product != null && rfq.Product.CategoryId != 0)
            {
                categoryId = rfq.Product.CategoryId;
                categoryName = rfq.Product.Category?.Name ?? "";
            }
            else
            {
               return RequestResult<List<RecommendedSupplierViewModel>>.Failure(ErrorCode.NotFound, "Product or Category not found for the RFQ.");
            }

            var suppliers = _supplierRepository.GetAllWithIncludes(s => s
                .Where(sup => sup.Products.Any(p => p.CategoryId == categoryId))
            ).ToList();
            

            var recommended = suppliers.Select(s => new RecommendedSupplierViewModel
            {
                SupplierId = s.Id,
                Name = s.Name,
                Rating = s.Rate != null && s.Rate.Any()
                    ? s.Rate.Average(r => r.RateNumber)
                    : 0

            }).ToList();


            return RequestResult<List<RecommendedSupplierViewModel>>.Success(recommended, "Recommended suppliers fetched successfully.");
        }
    }
}
