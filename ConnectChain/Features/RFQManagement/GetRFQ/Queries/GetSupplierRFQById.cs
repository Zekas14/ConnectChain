
using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel.RFQ.GetRFQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ConnectChain.Features.RFQManagement.GetRFQ.Queries
{
    public record GetSupplierRFQByIdQuery(int RfqId, string SupplierId) : IRequest<RequestResult<GetSupplierRFQByIdViewModel>>;

    public class GetSupplierRFQByIdQueryHandler : IRequestHandler<GetSupplierRFQByIdQuery, RequestResult<GetSupplierRFQByIdViewModel>>
    {
        private readonly IRepository<RFQ> _rfqRepository;

        public GetSupplierRFQByIdQueryHandler(IRepository<RFQ> rfqRepository)
        {
            _rfqRepository = rfqRepository;
        }

        public async Task<RequestResult<GetSupplierRFQByIdViewModel>> Handle(GetSupplierRFQByIdQuery request, CancellationToken cancellationToken)
        {
            var rfq = _rfqRepository.GetAllWithIncludes(q => q
                .Where(x => x.ID == request.RfqId&&x.Status==RfqStatus.Pending)
                .Include(x => x.Product).Include(p => p.Category)
                .Include(x => x.Attachments)
                .Include(x => x.SupplierAssignments).ThenInclude(sa => sa.Supplier)
            ).FirstOrDefault();

            if (rfq == null)
                return RequestResult<GetSupplierRFQByIdViewModel>.Failure(ErrorCode.NotFound, "RFQ not found.");

           
            var assignment = rfq.SupplierAssignments.FirstOrDefault(sa => sa.SupplierId == request.SupplierId);
            if (assignment == null)
                return RequestResult<GetSupplierRFQByIdViewModel>.Failure(ErrorCode.Forbidden, "You are not authorized to view this RFQ.");

            var viewModel = new GetSupplierRFQByIdViewModel
            {
                Id = rfq.ID,
                ProductName = rfq.ProductName,
                CategoryId = rfq.CategoryId ,
                CategoryName = rfq.Category?.Name ?? "",
                Description = rfq.Description,
                Quantity = rfq.Quantity,
                Unit = rfq.Unit,
                Deadline = rfq.Deadline,
                ShareBusinessCard = rfq.ShareBusinessCard,
                Status = rfq.Status,
                Attachments = rfq.Attachments.Select(a => new RfqAttachmentViewModel
                {
                    Id = a.ID,
                    FileUrl = a.FileUrl
                }).ToList(),
                SupplierAssignment = assignment == null ? null : new RfqSupplierAssignmentViewModel
                {
                    Id = assignment.ID,
                    SupplierId = assignment.SupplierId,
                    SupplierName = assignment.Supplier?.Name ?? "",
                    Status = assignment.Status
                }
            };

            return RequestResult<GetSupplierRFQByIdViewModel>.Success(viewModel, "RFQ fetched successfully.");
        }
    }

}
