using MediatR;

namespace ConnectChain.Features.OrderManagement.PlaceOrder.Events
{
    public record OrderPlacedEvent(string supplierId): INotification;
}
