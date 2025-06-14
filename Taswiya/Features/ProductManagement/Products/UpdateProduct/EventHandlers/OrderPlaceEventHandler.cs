using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.OrderManagement.PlaceOrder.Events;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.ProductManagement.Products.UpdateProduct.EventHandlers
{
    public class OrderPlacedEventHandler(IRepository<Product> repository,IMediator mediator) : INotificationHandler<OrderPlacedEvent>
    {
        private readonly IRepository<Product> repository = repository;
        private readonly IMediator mediator = mediator;

        public Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
        {
            var products = notification.Order.OrderItems.Select(p => p.Product);

        }
    }
}
