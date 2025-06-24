using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.RFQ.GetRFQ;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.RFQManagement.GetRFQ.Queries
{
    
    public record GetCustomerRFQByIdQuery(int RfqId, string CustomerId) : IRequest<RequestResult<GetCustomerRFQByIdViewModel>>;
    public class GetCustomerRFQByIdQueryHandler : IRequestHandler<GetCustomerRFQByIdQuery, RequestResult<GetCustomerRFQByIdViewModel>>
    {
        private readonly IRepository<RFQ> _rfqRepository;

        public GetCustomerRFQByIdQueryHandler(IRepository<RFQ> rfqRepository)
        {
            _rfqRepository = rfqRepository;
        }

        public async Task<RequestResult<GetCustomerRFQByIdViewModel>> Handle(GetCustomerRFQByIdQuery request, CancellationToken cancellationToken)
        {
            
            var rfq = _rfqRepository.GetAllWithIncludes(q => q
                .Where(x => x.ID == request.RfqId)
                .Include(x => x.Customer)
                .Include(x => x.Product).ThenInclude(p => p.Category)
                .Include(x => x.Attachments)
                .Include(x => x.SupplierAssignments).ThenInclude(sa => sa.Supplier)
            ).FirstOrDefault();

            if (rfq == null)
                return RequestResult<GetCustomerRFQByIdViewModel>.Failure(ErrorCode.NotFound, "RFQ not found.");

            
            if (rfq.CustomerId != request.CustomerId)
                return RequestResult<GetCustomerRFQByIdViewModel>.Failure(ErrorCode.Forbidden, "You are not authorized to view this RFQ.");

            var viewModel = new GetCustomerRFQByIdViewModel
            {
                Id = rfq.ID,
                CustomerId = rfq.CustomerId,
                CustomerName = rfq.Customer?.Name ?? "",
                ProductId = rfq.ProductId,
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
                SupplierAssignments = rfq.SupplierAssignments.Select(sa => new RfqSupplierAssignmentViewModel
                {
                    Id = sa.ID,
                    SupplierId = sa.SupplierId,
                    SupplierName = sa.Supplier?.Name ?? "",
                    Status = sa.Status.ToString()
                }).ToList()
            };

            return RequestResult<GetCustomerRFQByIdViewModel>.Success(viewModel, "RFQ fetched successfully.");
        }
    }
}

