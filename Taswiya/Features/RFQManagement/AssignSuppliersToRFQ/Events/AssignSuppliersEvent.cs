using MediatR;

namespace ConnectChain.Features.RFQManagement.AssignSuppliersToRFQ.Events
{
    public record  AssignSuppliersEvent(int RfqId, List<string> SupplierIds) : INotification;
}
