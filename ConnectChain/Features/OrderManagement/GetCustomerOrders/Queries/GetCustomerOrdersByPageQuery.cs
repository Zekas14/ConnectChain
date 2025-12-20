using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Order.GetCustomerOrders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.OrderManagement.GetCustomerOrders.Queries
{
    public record GetCustomerOrdersByPageQuery(string CustomerId, PaginationHelper PaginationParams) 
        : IRequest<RequestResult<IReadOnlyList<GetCustomerOrdersResponseViewModel>>>;
    
    public class GetCustomerOrdersByPageQueryHandler(IRepository<Order> repository) 
        : IRequestHandler<GetCustomerOrdersByPageQuery, RequestResult<IReadOnlyList<GetCustomerOrdersResponseViewModel>>>
    {
        private readonly IRepository<Order> _repository = repository;

        public async Task<RequestResult<IReadOnlyList<GetCustomerOrdersResponseViewModel>>> Handle(GetCustomerOrdersByPageQuery request, CancellationToken cancellationToken)
        {
            var allOrders =  _repository.Get(o => o.CustomerId == request.CustomerId)
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
                .Skip((request.PaginationParams.PageNumber - 1) * request.PaginationParams.PageSize)
                .Take(request.PaginationParams.PageSize)
                .ToList();

            if (allOrders.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetCustomerOrdersResponseViewModel>>.Failure(ErrorCode.NotFound, "No orders found for this customer.");
            }

              
            return RequestResult<IReadOnlyList<GetCustomerOrdersResponseViewModel>>.Success(allOrders, "Customer orders retrieved successfully.");
        }
    }
}
