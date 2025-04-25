using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.OrderManagement.PlaceOrder.Events;
using ConnectChain.Features.SupplierManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Order.PlaceOrder;
using MediatR;

namespace ConnectChain.Features.OrderManagement.PlaceOrder.Command
{
    public record PlaceOrderCommand(string CustomerId,decimal SubTotal ,decimal Discount , string Notes,string SupplierId , List<OrderItems> Items,string FcmToken) 
        : IRequest<RequestResult<bool>>;
    public class PlaceOrderCommandHandler(IMediator mediator, IRepository<Order> repository) : IRequestHandler<PlaceOrderCommand, RequestResult<bool>>
    {
        private readonly IMediator mediator = mediator;
        private readonly IRepository<Order> repository = repository;

        public async Task<RequestResult<bool>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var isSupplierExists = await mediator.Send(new IsSupplierExistsQuery(request.SupplierId), cancellationToken);
            if (!isSupplierExists.isSuccess)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Supplier Not Found");
            }
            var order = new Order
            {

                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,
                CreatedDate = DateTime.UtcNow,
                Notes = request.Notes,

                OrderItems = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity

                }).ToList(),
            };
            repository.Add(order);
            await mediator.Publish(new OrderPlacedEvent(order.ID,order.SupplierId,request.FcmToken),cancellationToken);
            return RequestResult<bool>.Success(true ,"Order Placed Successfully");
        }
    }
}
