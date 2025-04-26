using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.OrderManagement.PlaceOrder.Events
{
    public record OrderPlacedEvent(Order Order): INotification;
}
