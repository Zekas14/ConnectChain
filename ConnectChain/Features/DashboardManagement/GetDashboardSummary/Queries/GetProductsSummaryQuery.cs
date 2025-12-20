using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Models;
using ConnectChain.ViewModel.Dashboard.GetDashboardSummary;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.DashboardManagement.GetDashboardSummary.Queries
{
    public record GetProductsSummaryQuery(string SupplierId) : IRequest<ProductsSummaryResponseViewModel>;

    public class GetProductsSummaryQueryHandler : IRequestHandler<GetProductsSummaryQuery, ProductsSummaryResponseViewModel>
    {
        private readonly IRepository<Product> productRepository;

        public GetProductsSummaryQueryHandler(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<ProductsSummaryResponseViewModel> Handle(GetProductsSummaryQuery request, CancellationToken cancellationToken)
        {
           
            var products = await productRepository.Get(p => p.SupplierId == request.SupplierId).ToListAsync(cancellationToken);
            var totalProducts = products.Count;
            var lowStockProducts = products.Count(p => p.Stock.HasValue && p.Stock <= 10 && p.Stock > 0);
            var outOfStockProducts = products.Count(p => p.Stock.HasValue && p.Stock == 0);

            return new ProductsSummaryResponseViewModel
            {
                TotalProducts = totalProducts,
                LowStockProducts = lowStockProducts,
                OutOfStockProducts = outOfStockProducts
            };
        }
    }
}
