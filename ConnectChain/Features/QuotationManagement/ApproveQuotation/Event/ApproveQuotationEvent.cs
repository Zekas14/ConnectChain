using MediatR;

namespace ConnectChain.Features.QuotationManagement.ApproveQuotation.Event
{
    public record ApproveQuotationEvent(int QuotationId) : INotification;
    
}
