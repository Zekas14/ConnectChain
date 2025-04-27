using ConnectChain.Features.ProductManagement.GetSupplierProducts.Queries;
using ConnectChain.Helpers;
using ConnectChain.ViewModel.Product.GetSupplierProduct;
using MediatR;

namespace ConnectChain.Features.ProductManagement.SearchProduct._ََQueries
{
    public record SearchProductQuery (string SupplierId, string SearchKey) :IRequest<RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>>;
    public class SearchProductQueryHandler(IMediator mediator) : IRequestHandler<SearchProductQuery, RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>>
    {
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>> Handle(SearchProductQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SearchKey))
            {
                return RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>.Failure(ErrorCode.BadRequest, "Search key must not be empty");
            }
            var key = request.SearchKey.Trim().ToLower();
            var productsResult = await _mediator.Send(new GetSupplierProductsQuery(request.SupplierId),cancellationToken);
            if (!productsResult.isSuccess)
            {
                return RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>.Failure(productsResult.errorCode, productsResult.message);
            }
            var data = productsResult.data.AsQueryable().Where(p => p.Name!.Contains(key, StringComparison.CurrentCultureIgnoreCase)).ToList();
                return RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>.Success(data,productsResult.message);
        }
    }
}
