using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Order.GetCustomerOrders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.OrderManagement.GetCustomerOrders.Queries
{
    public record GetCustomerOrderByNumberQuery(string CustomerId, string OrderNumber) : IRequest<RequestResult<GetCustomerOrdersResponseViewModel>>;
    
    public class GetCustomerOrderByNumberQueryHandler(IRepository<Order> repository) 
        : IRequestHandler<GetCustomerOrderByNumberQuery, RequestResult<GetCustomerOrdersResponseViewModel>>
    {
        private readonly IRepository<Order> _repository = repository;

        public async Task<RequestResult<GetCustomerOrdersResponseViewModel>> Handle(GetCustomerOrderByNumberQuery request, CancellationToken cancellationToken)
        {
            var orders =  _repository.Get(o => o.CustomerId == request.CustomerId && o.OrderNumber ==Guid.Parse( request.OrderNumber))
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Images)
                .Include(o => o.Supplier);

            if (!orders.Any())
            {
                return RequestResult<GetCustomerOrdersResponseViewModel>.Failure(ErrorCode.NotFound, "Order not found.");
            }

            var aggregatedOrder = new GetCustomerOrdersResponseViewModel
            {
                OrderNumber = Guid.Parse(request.OrderNumber),
                OrderDate = orders.First().CreatedDate,
                Status = orders.First().Status.ToString(),
                PaymentMethod = orders.First().PaymentMethod,
                SubTotal = orders.Sum(o => o.SubTotal),
                DeliveryFees = orders.Sum(o => o.DeliveryFees),
                Discount = orders.Sum(o => o.Discount),
                TotalAmount = orders.Sum(o => o.TotalAmount),
                TotalItems = orders.SelectMany(o => o.OrderItems).Sum(oi => oi.Quantity),
                Products = orders.SelectMany(o => o.OrderItems.Select(oi => new CustomerOrderProductViewModel
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.Product.Price,
                    TotalPrice = oi.Quantity * oi.Product.Price,
                    ProductImage = oi.Product.Images.Where(i => !i.Deleted).Select(i => i.Url).FirstOrDefault() ?? string.Empty,
                    SupplierName = o.Supplier.Name
                })).ToList()
            };

            return RequestResult<GetCustomerOrdersResponseViewModel>.Success(aggregatedOrder, "Order retrieved successfully.");
        }
    }
}
