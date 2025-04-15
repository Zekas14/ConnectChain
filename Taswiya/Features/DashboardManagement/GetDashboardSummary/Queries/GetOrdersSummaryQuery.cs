using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Models;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel.Dashboard.GetDashboardSummary;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.DashboardManagement.GetDashboardSummary.Queries
{
    public record GetOrdersSummaryQuery(string SupplierId) : IRequest<OrdersSummaryResponseViewModel>;

    public class GetOrdersSummaryQueryHandler(IRepository<Order> orderRepository)
        : IRequestHandler<GetOrdersSummaryQuery, OrdersSummaryResponseViewModel>
    {
        private readonly IRepository<Order> orderRepository = orderRepository;

        public async Task<OrdersSummaryResponseViewModel> Handle(GetOrdersSummaryQuery request, CancellationToken cancellationToken)
        {
            var completedOrders = await orderRepository.Get(o => o.SupplierId == request.SupplierId && o.Status == OrderStatus.Completed).CountAsync(cancellationToken);
            var pendingOrders = await orderRepository.Get(o => o.SupplierId == request.SupplierId && o.Status == OrderStatus.Pending).CountAsync(cancellationToken);
            var rejectedOrders = await orderRepository.Get(o => o.SupplierId == request.SupplierId && o.Status == OrderStatus.Rejected).CountAsync(cancellationToken);

            return new OrdersSummaryResponseViewModel
            {
                CompletedOrders = completedOrders,
                PendingOrders = pendingOrders,
                RejectedOrders = rejectedOrders
            };
        }
    }
}
