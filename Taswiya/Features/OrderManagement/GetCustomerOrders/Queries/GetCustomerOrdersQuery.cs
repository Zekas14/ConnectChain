using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Order.GetCustomerOrders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.OrderManagement.GetCustomerOrders.Queries
{
    public record GetCustomerOrdersQuery(string CustomerId) : IRequest<RequestResult<IReadOnlyList<GetCustomerOrdersResponseViewModel>>>;
    
    public class GetCustomerOrdersQueryHandler(IRepository<Order> repository) 
        : IRequestHandler<GetCustomerOrdersQuery, RequestResult<IReadOnlyList<GetCustomerOrdersResponseViewModel>>>
    {
        private readonly IRepository<Order> _repository = repository;

        public async Task<RequestResult<IReadOnlyList<GetCustomerOrdersResponseViewModel>>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = _repository.Get(o => o.CustomerId == request.CustomerId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Images)
                .Include(o => o.Supplier)
                .OrderByDescending(o => o.CreatedDate)
                  .GroupBy(o => o.OrderNumber)
                .Select(group => new GetCustomerOrdersResponseViewModel
                {
                    OrderNumber = group.Key,
                    OrderDate = group.First().CreatedDate,
                    Status = group.First().Status.ToString(),
                    PaymentMethod = group.First().PaymentMethod,
                    SubTotal = group.Sum(o => o.SubTotal),
                    DeliveryFees = group.Sum(o => o.DeliveryFees),
                    Discount = group.Sum(o => o.Discount),
                    TotalAmount = group.Sum(o => o.TotalAmount),
                    TotalItems = group.SelectMany(o => o.OrderItems).Sum(oi => oi.Quantity),
                    Products = group.SelectMany(o => o.OrderItems.Select(oi => new CustomerOrderProductViewModel
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.Product.Price,
                        TotalPrice = oi.Quantity * oi.Product.Price,
                        ProductImage = oi.Product.Images.Where(i => !i.Deleted).Select(i => i.Url).FirstOrDefault() ?? string.Empty,
                        SupplierName = o.Supplier.Name
                    })).ToList()
                })
                .OrderByDescending(o => o.OrderDate)
                .ToList();


            if (orders.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetCustomerOrdersResponseViewModel>>.Failure(ErrorCode.NotFound, "No orders found for this customer.");
            }

            //
            return RequestResult<IReadOnlyList<GetCustomerOrdersResponseViewModel>>.Success(orders, "Customer orders retrieved successfully.");
        }
    }
}
