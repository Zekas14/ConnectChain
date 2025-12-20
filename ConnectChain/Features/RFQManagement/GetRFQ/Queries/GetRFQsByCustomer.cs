using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.RFQ.GetRFQ;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.RFQManagement.GetRFQ.Queries
{
    public record GetRFQsByCustomerQuery(string CustomerId) : IRequest<RequestResult<List<GetCustomerRFQByIdViewModel>>>;

    public class GetRFQsByCustomerQueryHandler : IRequestHandler<GetRFQsByCustomerQuery, RequestResult<List<GetCustomerRFQByIdViewModel>>>
    {
        private readonly IRepository<RFQ> _rfqRepository;

        public GetRFQsByCustomerQueryHandler(IRepository<RFQ> rfqRepository)
        {
            _rfqRepository = rfqRepository;
        }

        public async Task<RequestResult<List<GetCustomerRFQByIdViewModel>>> Handle(GetRFQsByCustomerQuery request, CancellationToken cancellationToken)
        {
            var rfqs = _rfqRepository.GetAllWithIncludes(q => q
                .Where(x => x.CustomerId == request.CustomerId)
                .Include(x => x.Customer)
                .Include(x => x.Product)
                .Include(p => p.Category)
                .Include(x => x.Attachments)
                .Include(x => x.SupplierAssignments).ThenInclude(sa => sa.Supplier)
            ).ToList();

            var result = rfqs.Select(rfq => new GetCustomerRFQByIdViewModel
            {
                Id = rfq.ID,
                CustomerId = rfq.CustomerId,
                CustomerName = rfq.Customer?.Name ?? "",
                ProductId = rfq.ProductId,
                ProductName = rfq.ProductName,
                CategoryId = rfq.CategoryId,
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
                SupplierAssignments = rfq.SupplierAssignments.Select(sa => new RfqSupplierAssignmentViewModel
                {
                    Id = sa.ID,
                    SupplierId = sa.SupplierId,
                    SupplierName = sa.Supplier?.Name ?? "",
                    Status = sa.Status
                }).ToList()
            }).ToList();

            return RequestResult<List<GetCustomerRFQByIdViewModel>>.Success(result, "RFQs fetched successfully.");
        }
    }
}
