using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.OrderManagement.CancelOrder.Events
{
    public record OrderCancelledEvent(Guid OrderNumber, string CustomerId, List<string> SupplierIds) : INotification;
}
