using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.RFQ.GetRFQ;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.RFQManagement.GetRFQ.Queries
{
    public record GetRFQsBySupplierQuery(string SupplierId) : IRequest<RequestResult<List<GetSupplierRFQByIdViewModel>>>;

    public class GetRFQsBySupplierQueryHandler : IRequestHandler<GetRFQsBySupplierQuery, RequestResult<List<GetSupplierRFQByIdViewModel>>>
    {
        private readonly IRepository<RFQ> _rfqRepository;

        public GetRFQsBySupplierQueryHandler(IRepository<RFQ> rfqRepository)
        {
            _rfqRepository = rfqRepository;
        }

        public async Task<RequestResult<List<GetSupplierRFQByIdViewModel>>> Handle(GetRFQsBySupplierQuery request, CancellationToken cancellationToken)
        {
            var rfqs = _rfqRepository.GetAllWithIncludes(q => q
                .Include(x => x.Product).ThenInclude(p => p.Category)
                .Include(x => x.Attachments)
                .Include(x => x.SupplierAssignments).ThenInclude(sa => sa.Supplier)
            )
            .Where(rfq => rfq.SupplierAssignments.Any(sa => sa.SupplierId == request.SupplierId))
            .ToList();

            var result = rfqs.Select(rfq =>
            {
                var assignment = rfq.SupplierAssignments.FirstOrDefault(sa => sa.SupplierId == request.SupplierId);
                return new GetSupplierRFQByIdViewModel
                {
                    Id = rfq.ID,
                    ProductName = rfq.ProductName,
                    CategoryId = rfq.Product?.CategoryId ?? 0,
                    CategoryName = rfq.Product?.Category?.Name ?? "",
                    Description = rfq.Description,
                    Quantity = rfq.Quantity,
                    Unit = rfq.Unit,
                    Deadline = rfq.Deadline,
                    ShareBusinessCard = rfq.ShareBusinessCard,
                    Status = rfq.Status.ToString(),
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
                        Status = assignment.Status.ToString()
                    }
                };
            }).ToList();

            return RequestResult<List<GetSupplierRFQByIdViewModel>>.Success(result, "RFQs fetched successfully.");
        }
    }
}
