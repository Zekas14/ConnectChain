using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.SupplierManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel.Order.GetSupplierOrder;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace ConnectChain.Features.OrderManagement.GetSupplierOrders.Queries
{
    public record GetSupplierOrdersQuery(PaginationHelper Pagination , string SupplierId) : IRequest<RequestResult<IReadOnlyList<GetSupplierOrdersResponseViewModel>>>
    {
    }
    public class GetSupplierOrdersQueryHandler(IRepository<Order> repository,IMediator mediator)
        : IRequestHandler<GetSupplierOrdersQuery, RequestResult<IReadOnlyList<GetSupplierOrdersResponseViewModel>>>
    {
        private readonly IRepository<Order> repository = repository;
        private readonly IMediator mediator = mediator;

        public async Task<RequestResult<IReadOnlyList<GetSupplierOrdersResponseViewModel>>> Handle(GetSupplierOrdersQuery request, CancellationToken cancellationToken)
        {
             var isSupplierFoundResult = await mediator.Send(new IsUserExistsQuery(request.SupplierId));
            if (!isSupplierFoundResult.isSuccess)
            {
                return RequestResult<IReadOnlyList<GetSupplierOrdersResponseViewModel>>.Failure(isSupplierFoundResult.errorCode,  "Supplier Not Found");
            }
                var orders = repository.GetByPage(o => o.SupplierId == request.SupplierId,request.Pagination)
                .Include(o => o.OrderItems)
                .Include(o => o.Customer)
                .Select(o => new GetSupplierOrdersResponseViewModel
                {
                    Id = o.ID,
                    CustomerName = o.Customer.UserName,
                    OrderDate = o.CreatedDate.ToString("yyyy-MM-dd"),
                    TotalAmount = o.TotalAmount,
                    OrderStatus = o.Status.ToString(),
                    Products = o.OrderItems.Select(oi => oi.Product.Name).ToList(),
                });
            if (orders.IsNullOrEmpty())
            {
                return RequestResult< IReadOnlyList<GetSupplierOrdersResponseViewModel>>.Failure(ErrorCode.NotFound,"No Orders yet");
            }
            return RequestResult<IReadOnlyList<GetSupplierOrdersResponseViewModel>>.Success(orders.ToList(), "Orders retrieved successfully");
        }
    }
}
