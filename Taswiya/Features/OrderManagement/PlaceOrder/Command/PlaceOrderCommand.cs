using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.OrderManagement.PlaceOrder.Events;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Features.SupplierManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Order.PlaceOrder;
using MediatR;

namespace ConnectChain.Features.OrderManagement.PlaceOrder.Command
{
    public record PlaceOrderCommand(string CustomerId,decimal SubTotal ,decimal Discount , string Notes , List<OrderItems> Items) 
        : IRequest<RequestResult<bool>>;
    public class PlaceOrderCommandHandler(IMediator mediator, IRepository<Order> repository)
     : IRequestHandler<PlaceOrderCommand, RequestResult<bool>>
    {
        private readonly IMediator mediator = mediator;
        private readonly IRepository<Order> repository = repository;

        public async Task<RequestResult<bool>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();

            var productsResult = await mediator.Send(new GetExistingProductsQuery(productIds), cancellationToken);
            if (!productsResult.isSuccess)
            {
                return RequestResult<bool>.Failure(productsResult.errorCode, productsResult.message);
            }
            var products = productsResult.data;
            var itemsWithSupplier = request.Items.Select(i => new
            {
                Item = i,
                products.First(p => p.ID == i.ProductId).SupplierId
            });
            var groupedBySupplier = itemsWithSupplier
                .GroupBy(x => x.SupplierId)
                .ToList();
            foreach (var group in groupedBySupplier)
            {
                var supplierId = group.Key;

                var orderItems = group.Select(g => new OrderItem
                {
                    ProductId = g.Item.ProductId,
                    Quantity = g.Item.Quantity,
                }).ToList();

                decimal subTotal = orderItems.Sum(oi =>
                {
                var product = products.First(p => p.ID == oi.ProductId);
                    return product.Price * oi.Quantity;
                });
                var order = new Order
                {
                    OrderNumber = Guid.NewGuid(),
                    CustomerId = request.CustomerId,
                    SupplierId = supplierId!,

                    CreatedDate = DateTime.UtcNow,
                    Notes = request.Notes,
                    Discount = request.Discount,
                    SubTotal = subTotal,
                    OrderItems = orderItems
                };
                repository.Add(order);
                await mediator.Publish(new OrderPlacedEvent(order), cancellationToken);
            }

            return RequestResult<bool>.Success(true, "Orders placed successfully");
        }
    }
}
