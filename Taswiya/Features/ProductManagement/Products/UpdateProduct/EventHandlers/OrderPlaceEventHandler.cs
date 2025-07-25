﻿using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.OrderManagement.PlaceOrder.Events;
using ConnectChain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.ProductManagement.Products.UpdateProduct.EventHandlers
{
    public class OrderPlacedEventHandler(IRepository<Product> repository, IMediator mediator) : INotificationHandler<OrderPlacedEvent>
    {
        private readonly IRepository<Product> repository = repository;

        public async Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
        {
            var orderItems = notification.Order.OrderItems;
            foreach (var item in orderItems)
            {
                await repository.Table.Where(p => p.ID == item.ProductId)
                    .ExecuteUpdateAsync(e => e.SetProperty(p => p.Stock, p => p.Stock - item.Quantity), cancellationToken);
            }
        }
    }
}
