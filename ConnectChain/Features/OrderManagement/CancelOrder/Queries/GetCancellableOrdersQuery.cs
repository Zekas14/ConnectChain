using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.OrderManagement.CancelOrder.Queries
{
    public record GetCancellableOrdersQuery(string CustomerId, PaginationHelper? PaginationParams = null) 
        : IRequest<RequestResult<IReadOnlyList<CancellableOrderResponseViewModel>>>;
    
    public class CancellableOrderResponseViewModel
    {
        public Guid OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public double HoursRemaining { get; set; }
        public bool CanCancel { get; set; }
        public int TotalItems { get; set; }
        public List<string> SupplierNames { get; set; } = new();
    }
    
    public class GetCancellableOrdersQueryHandler(IRepository<Order> repository) 
        : IRequestHandler<GetCancellableOrdersQuery, RequestResult<IReadOnlyList<CancellableOrderResponseViewModel>>>
    {
        private readonly IRepository<Order> _repository = repository;

        public async Task<RequestResult<IReadOnlyList<CancellableOrderResponseViewModel>>> Handle(GetCancellableOrdersQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                return RequestResult<IReadOnlyList<CancellableOrderResponseViewModel>>.Failure(ErrorCode.BadRequest, "Customer ID cannot be empty.");
            }

            // Get orders that are potentially cancellable (Pending or Accepted status)
            var orders = await _repository.Get(o => o.CustomerId == request.CustomerId && 
                                                   (o.Status == OrderStatus.Pending || o.Status == OrderStatus.Accepted))
                .Include(o => o.Supplier)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync(cancellationToken);

            if (orders.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<CancellableOrderResponseViewModel>>.Failure(ErrorCode.NotFound, "No potentially cancellable orders found.");
            }

            // Group by order number and check cancellation eligibility
            var groupedOrders = orders
                .GroupBy(o => o.OrderNumber)
                .Select(group => 
                {
                    var firstOrder = group.First();
                    var timeSinceCreation = DateTime.Now - firstOrder.CreatedDate;
                    var hoursRemaining = 24 - timeSinceCreation.TotalHours;
                    var canCancel = timeSinceCreation.TotalDays <= 1;

                    return new CancellableOrderResponseViewModel
                    {
                        OrderNumber = group.Key,
                        OrderDate = firstOrder.CreatedDate,
                        Status = firstOrder.Status.ToString(),
                        TotalAmount = group.Sum(o => o.TotalAmount),
                        TotalItems = group.SelectMany(o => o.OrderItems).Sum(oi => oi.Quantity),
                        SupplierNames = group.Select(o => o.Supplier.Name ?? "Unknown").Distinct().ToList(),
                        HoursRemaining = Math.Max(0, hoursRemaining),
                        CanCancel = canCancel
                    };
                })
                .OrderByDescending(o => o.OrderDate)
                .AsQueryable();

            // Apply pagination if provided
            if (request.PaginationParams != null)
            {
                groupedOrders = groupedOrders.Skip((request.PaginationParams.PageNumber - 1) * request.PaginationParams.PageSize)
                                           .Take(request.PaginationParams.PageSize);
            }

            var result = groupedOrders.ToList();

            if (result.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<CancellableOrderResponseViewModel>>.Failure(ErrorCode.NotFound, "No cancellable orders found.");
            }

            return RequestResult<IReadOnlyList<CancellableOrderResponseViewModel>>.Success(result, "Cancellable orders retrieved successfully.");
        }
    }
}
