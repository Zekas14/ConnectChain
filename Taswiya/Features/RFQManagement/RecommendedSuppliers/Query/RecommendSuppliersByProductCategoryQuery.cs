using ConnectChain.Data.Context;
using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.RFQ.GetRecommendedSuppliers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.RFQManagement.RecommendedSuppliers.Query
{
    public record RecommendSuppliersByProductCategoryQuery(int RfqId) : IRequest<RequestResult<List<RecommendedSupplierViewModel>>>;

    public class RecommendSuppliersByProductCategoryQueryHandler : IRequestHandler<RecommendSuppliersByProductCategoryQuery, RequestResult<List<RecommendedSupplierViewModel>>>
    {
        private readonly IRepository<RFQ> _rfqRepository;
        private readonly ConnectChainDbContext _context;
        private readonly IRepository<Category> _categoryRepository;

        public RecommendSuppliersByProductCategoryQueryHandler(
            IRepository<RFQ> rfqRepository,
            ConnectChainDbContext context,
            IRepository<Category> categoryRepository)
        {
            _rfqRepository = rfqRepository;
            _context = context;
            _categoryRepository = categoryRepository;
        }

        public async Task<RequestResult<List<RecommendedSupplierViewModel>>> Handle(RecommendSuppliersByProductCategoryQuery request, CancellationToken cancellationToken)
        {
            var rfq = _rfqRepository.GetAllWithIncludes(q => q.Where(x => x.ID == request.RfqId)
             .Select(x => new RFQ
             {
                 ID = x.ID,
                 CategoryId = x.CategoryId,
                 ProductId = x.ProductId,
                 Product = x.Product
             })).FirstOrDefault();

            if (rfq.CategoryId <= 0)
                return RequestResult<List<RecommendedSupplierViewModel>>.Failure(ErrorCode.NotFound, "Invalid category for the RFQ.");

            if (rfq == null)
                return RequestResult<List<RecommendedSupplierViewModel>>.Failure(ErrorCode.NotFound, "RFQ not found.");

            var categoryExists = _categoryRepository.Get(c => c.ID == rfq.CategoryId).Any();
            if (!categoryExists)
            return RequestResult<List<RecommendedSupplierViewModel>>.Failure(ErrorCode.NotFound, "Category not found for the RFQ.");

            List<Supplier> suppliers;

            if (rfq.ProductId.HasValue)
            {

                suppliers = _context.Suppliers.Where(sup => sup.Products.Any(p => p.ID == rfq.ProductId.Value && p.CategoryId == rfq.CategoryId)).ToList();
            }
            else
            {
                suppliers = _context.Suppliers.Where(sup => sup.Products.Any(p => p.CategoryId == rfq.CategoryId)).ToList();
            }


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
