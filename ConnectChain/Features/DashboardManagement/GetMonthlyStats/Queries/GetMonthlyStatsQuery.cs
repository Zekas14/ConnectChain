using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Dashboard.GetMonthlyStats;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.DashboardManagement.GetMonthlyStats.Queries
{
    public record GetMonthlyStatsQuery(string SupplierId, int Month, int Year) : IRequest<GetMonthlyStatsResponseViewModel>;

    public class GetMonthlyStatsQueryHandler(IRepository<Order> repository)
        : IRequestHandler<GetMonthlyStatsQuery, GetMonthlyStatsResponseViewModel>
    {
        private readonly IRepository<Order> repository = repository;

        public async Task<GetMonthlyStatsResponseViewModel> Handle(GetMonthlyStatsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("----------------------------mango befo --------------------------------");
                var orders = await repository.Get(o => o.SupplierId == request.SupplierId
                                                       && o.CreatedDate.Month == request.Month
                                                       && o.CreatedDate.Year == request.Year)
                                             .Include(o => o.OrderItems)
                                             .ThenInclude(oi => oi.Product)
                                             .ToListAsync(cancellationToken);
                Console.WriteLine("----------------------------mango after --------------------------------");

                if (!orders.Any())
                {
                    return new GetMonthlyStatsResponseViewModel
                    {
                        TopSoldProductId = null,
                        TopSoldProductName = null,
                        AverageOrderTotal = 0,
                        TotalRevenues = 0
                    };
                }

                var totalRevenues = orders.Sum(o => o.TotalAmount);
                var averageOrderTotal = orders.Average(o => o.TotalAmount);
                var orderedItems = orders.SelectMany(o => o.OrderItems)
                                          .GroupBy(oi => new { oi.Product.ID, oi.Product.Name })
                                          .OrderByDescending(g => g.Sum(oi => oi.Quantity));
                var topSoldProduct = orderedItems.FirstOrDefault();

                return new GetMonthlyStatsResponseViewModel
                {
                    TopSoldProductId = topSoldProduct?.Key.ID,
                    TopSoldProductName = topSoldProduct?.Key.Name,
                    AverageOrderTotal = averageOrderTotal,
                    TotalRevenues = totalRevenues
                };
            }
            catch (DbUpdateException ex)
            {
                
                throw new Exception("A database error occurred while retrieving monthly stats.", ex);
            }
            catch (Exception ex)
            {
                
                throw new Exception("An unexpected error occurred while retrieving monthly stats.", ex);
            }
        }
    }
}
