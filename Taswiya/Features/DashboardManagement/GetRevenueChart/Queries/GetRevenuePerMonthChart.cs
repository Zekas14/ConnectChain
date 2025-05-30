﻿using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Models;
using ConnectChain.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.DashboardManagement.GetRevenueChart.Queries
{
    public record GetRevenuePerMonthChartQuery(string SupplierId, int Year) : IRequest<Dictionary<int, decimal>>;

    public class GetRevenuePerMonthChartQueryHandler(IRepository<Order> repository)
        : IRequestHandler<GetRevenuePerMonthChartQuery, Dictionary<int, decimal>>
    {
        private readonly IRepository<Order> repository = repository;

        public async Task<Dictionary<int, decimal>> Handle(GetRevenuePerMonthChartQuery request, CancellationToken cancellationToken)
        {
            var orders = await repository.Get(o => o.SupplierId == request.SupplierId && o.CreatedDate.Year == request.Year)
                                         .ToListAsync(cancellationToken);

            if (!orders.Any())
            {
                return new Dictionary<int, decimal>();
            }

            var revenuePerMonth = orders.GroupBy(o => o.CreatedDate.Month)
                                        .ToDictionary(
                                            g => g.Key,
                                            g => g.Sum(o => o.TotalAmount)
                                        );

            return revenuePerMonth;
        }
    }
}
