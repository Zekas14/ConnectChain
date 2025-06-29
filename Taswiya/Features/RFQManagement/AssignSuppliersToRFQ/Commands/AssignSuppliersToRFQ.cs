using ConnectChain.Data.Context;
using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.NotificationManagement.SendNotification.Command;
using ConnectChain.Features.RFQManagement.AssignSuppliersToRFQ.Events;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.RFQManagement.AssignSuppliersToRFQ.Commands

{

    public record AssignSuppliersToRFQCommand(int RfqId, List<string> SupplierIds) : IRequest<RequestResult<bool>>;

    public class AssignSuppliersToRFQCommandHandler : IRequestHandler<AssignSuppliersToRFQCommand, RequestResult<bool>>
    {
        private readonly IRepository<RfqSupplierAssignment> _assignmentRepository;
      
        private readonly ConnectChainDbContext _context;
        private readonly IMediator _mediator;

        public AssignSuppliersToRFQCommandHandler(
            IRepository<RfqSupplierAssignment> assignmentRepository,
            ConnectChainDbContext context,
            IMediator mediator)
        {
            _assignmentRepository = assignmentRepository;
            _context = context;
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

                //var suppliers = _context.Suppliers.Where(s => request.SupplierIds.Contains(s.Id)).ToList();
                //var deviceTokens = suppliers
                //    .Select(s => s.FcmToken)
                //    .Where(token => !string.IsNullOrEmpty(token))
                //    .ToList();

            
                await _mediator.Publish(new AssignSuppliersEvent(request.RfqId, request.SupplierIds), cancellationToken);

                return RequestResult<bool>.Success(true, "Suppliers assigned and notified successfully.");
            }
            catch (Exception ex)
            {
                return RequestResult<bool>.Failure(ErrorCode.InternalServerError, $"Failed to assign suppliers: {ex.Message}");
            }
        }
    }
}
