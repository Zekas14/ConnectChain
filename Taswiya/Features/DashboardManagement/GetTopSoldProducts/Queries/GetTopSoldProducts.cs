using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Models;
using ConnectChain.ViewModel.Dashboard.GetTopSoldProducts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.DashboardManagement.GetTopSoldProducts.Queries
{
    public record GetTopSoldProductsQuery(string SupplierId, int Year, int Month, int Limit) : IRequest<IReadOnlyList<GetTopSoldProductsResponseViewModel>>;

    public class GetTopSoldProductsQueryHandler(IRepository<Order> repository)
        : IRequestHandler<GetTopSoldProductsQuery, IReadOnlyList<GetTopSoldProductsResponseViewModel>>
    {
        private readonly IRepository<Order> repository = repository;

        public async Task<IReadOnlyList<GetTopSoldProductsResponseViewModel>> Handle(GetTopSoldProductsQuery request, CancellationToken cancellationToken)
        {
            var orders = await repository.Get(o => o.SupplierId == request.SupplierId 
                                                   && o.CreatedDate.Year == request.Year 
                                                   && o.CreatedDate.Month == request.Month)
                                         .Include(o => o.OrderItems)
                                         .ThenInclude(oi => oi.Product)
                                         .ThenInclude(p => p.Images)
                                         .ToListAsync(cancellationToken);

            var topSoldProducts = orders.SelectMany(o => o.OrderItems)
                                        .GroupBy(oi => new { oi.Product.ID, oi.Product.Name, oi.Product.Images })
                                        .OrderByDescending(g => g.Sum(oi => oi.Quantity))
                                        .Take(request.Limit)
                                        .Select(g => new GetTopSoldProductsResponseViewModel
                                        {
                                            ProductId = g.Key.ID,
                                            ProductName = g.Key.Name,
                                            ImageUrls = g.Key.Images.Select(img => img.Url).ToList(),
                                            TotalSoldQuantity = g.Sum(oi => oi.Quantity)
                                        })
                                        .ToList();

            return topSoldProducts;
        }
    }
}
