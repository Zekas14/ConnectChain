using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.GetProduct;
using MediatR;

namespace ConnectChain.Features.ProductManagement.GetProductById.Queries
{
    public record GetProductByIdQuery(int Id) : IRequest<RequestResult<GetProductResponseViewModel>>;
    public class GetProductByIdQueryHandler(IRepository<Product> repository,IMediator mediator) : IRequestHandler<GetProductByIdQuery, RequestResult<GetProductResponseViewModel>>
    {
        private readonly IRepository<Product> repository = repository;
        private readonly IMediator mediator = mediator;

        public async Task<RequestResult<GetProductResponseViewModel>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var productExistResult = await mediator.Send(new IsProductExistQuery(request.Id),cancellationToken);
            if (!productExistResult.isSuccess)
            {
                return RequestResult<GetProductResponseViewModel>.Failure(productExistResult.errorCode, productExistResult.message);
            }
            var requestResult = new GetProductResponseViewModel
            {
                Id = productExistResult.data.ID,
                Name = productExistResult.data.Name,
                Description = productExistResult.data.Description,
                Price = productExistResult.data.Price,
                Image = productExistResult.data.Image
            };
            return RequestResult<GetProductResponseViewModel>.Success(requestResult);
        }
    }
}
