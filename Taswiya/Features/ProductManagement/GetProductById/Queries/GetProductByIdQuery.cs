using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.GetProduct;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.ProductManagement.GetProductById.Queries
{
    public record GetProductByIdQuery(int Id) : IRequest<RequestResult<GetProductResponseViewModel>>;
    public class GetProductByIdQueryHandler(IRepository<Product> repository,IMediator mediator) : IRequestHandler<GetProductByIdQuery, RequestResult<GetProductResponseViewModel>>
    {
        private readonly IRepository<Product> _repository = repository;

        public async Task<RequestResult<GetProductResponseViewModel>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product =  await _repository.GetAll()
                .Where(p=>p.ID==request.Id)
                .Include(p => p.Images)
                .Select(product => new GetProductResponseViewModel
                {
                    Id = product.ID,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Images = product.Images.Select(x => x.Url).ToList()

                }).FirstOrDefaultAsync(cancellationToken);
            if (product is null)
            {
                return RequestResult<GetProductResponseViewModel>.Failure(ErrorCode.NotFound, "Product Not Found");
            }
           /* var requestResult = new GetProductResponseViewModel
            {
                Id = product.ID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Images = product.Images.Select(x => x.Url).ToList()?? new()
           };*/
            return RequestResult<GetProductResponseViewModel>.Success(product);
        }
    }
}
