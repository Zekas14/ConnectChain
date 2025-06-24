using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.NotificationManagement.SendNotification.Command;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.RFQManagement.AssignSuppliersToRFQ.Commands

{

    public record AssignSuppliersToRFQCommand(int RfqId, List<string> SupplierIds) : IRequest<RequestResult<bool>>;

    public class AssignSuppliersToRFQCommandHandler : IRequestHandler<AssignSuppliersToRFQCommand, RequestResult<bool>>
    {
        private readonly IRepository<RfqSupplierAssignment> _assignmentRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IMediator _mediator;

        public AssignSuppliersToRFQCommandHandler(
            IRepository<RfqSupplierAssignment> assignmentRepository,
            IRepository<Supplier> supplierRepository,
            IMediator mediator)
        {
            _assignmentRepository = assignmentRepository;
            _supplierRepository = supplierRepository;
            _mediator = mediator;
        }

        public async Task<RequestResult<bool>> Handle(AssignSuppliersToRFQCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.SupplierIds == null || !request.SupplierIds.Any())
                    return RequestResult<bool>.Failure(ErrorCode.BadRequest, "No suppliers specified.");

                var assignments = request.SupplierIds.Select(supplierId => new RfqSupplierAssignment
                {
                    RfqId = request.RfqId,
                    SupplierId = supplierId,
                    Status = Models.Enums.RfqStatus.Pending
                }).ToList();

                _assignmentRepository.AddRange(assignments);
                await _assignmentRepository.SaveChangesAsync();

                var suppliers = _supplierRepository.Get(s => request.SupplierIds.Contains(s.Id)).ToList();
                var deviceTokens = suppliers
                    .Select(s => s.FcmToken)
                    .Where(token => !string.IsNullOrEmpty(token))
                    .ToList();

                if (deviceTokens.Any())
                {
                    var title = "New RFQ Received";
                    var body = $"You have been assigned to a new RFQ (ID: {request.RfqId}).";
                    var notificationResult = await _mediator.Send(new SendNotificationsCommand(deviceTokens, title, body, "RFQ"), cancellationToken);
                    if (!notificationResult.isSuccess)
                    {
                        return RequestResult<bool>.Failure(notificationResult.errorCode, $"Suppliers assigned, but failed to send notifications: {notificationResult.message}");
                    }
                }

                return RequestResult<bool>.Success(true, "Suppliers assigned and notified successfully.");
            }
            catch (Exception ex)
            {
                return RequestResult<bool>.Failure(ErrorCode.InternalServerError, $"Failed to assign suppliers: {ex.Message}");
            }
        }
    }
}
