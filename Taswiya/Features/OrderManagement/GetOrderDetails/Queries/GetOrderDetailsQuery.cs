using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Order.GetOrderDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.OrderManagement.GetOrderDetails.Queries
{
    public record GetOrderDetailsQuery(int OrderId) : IRequest<RequestResult<GetOrderDetailsResponseViewModel>>;
    public class GetOrderDetailsQueryQueryHandler(IRepository<Order> repository) : IRequestHandler<GetOrderDetailsQuery, RequestResult<GetOrderDetailsResponseViewModel>>
    {
        private readonly IRepository<Order> repository = repository;

        public async Task<RequestResult<GetOrderDetailsResponseViewModel>> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            var order = repository.GetByIDWithIncludes(request.OrderId, o => o
            .Include(oi => oi.OrderItems)
            .ThenInclude(oi=>oi.Product)
            .ThenInclude(p=>p.Images)
            .Include(oi=>oi.Customer)
            );
            if (order is null)
            {
                return RequestResult<GetOrderDetailsResponseViewModel>.Failure(ErrorCode.NotFound, "Order not found");
            }
            GetOrderDetailsResponseViewModel data = new()
            {
                OrderNumber = order.ID,
                OrderDate = order.CreatedDate,
                CustomerAddress = order.Customer.Address,
                CustomerName = order.Customer.Name,
                DeliveryFees = order.DeliveryFees,
                Discount = order.Discount,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status.ToString(),
                Products = order.OrderItems.Select(oi => new ProductsOrderedViewModel
                {
                    ProductName = oi.Product!.Name,
                    Quantity = oi.Quantity,
                    ImageUrl = oi.Product.Images.Select(i => i.Url).ToList() ?? [],
                    UnitPrice = oi.UnitPrice,
                }).ToList(),
                SubTotal = order.OrderItems.Select(oi => oi.UnitPrice).Sum(),
            };
            
            return RequestResult<GetOrderDetailsResponseViewModel>.Success(data,"Success");


        }
    }
}
