using MediatR;

namespace ConnectChain.Features.OrderManagement.PlaceOrder.Events
{
    public record OrderPlacedEvent(int OrderId,string SupplierId , string FcmToken): INotification;
}
