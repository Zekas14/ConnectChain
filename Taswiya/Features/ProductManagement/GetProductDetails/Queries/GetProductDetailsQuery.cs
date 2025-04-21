using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.GetProductDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.ProductManagement.GetProductDetails.Queries
{
    public record GetProductDetailsQuery(int Id) : IRequest<RequestResult<GetProductDetailsResponseViewModel>>;
    public class GetProductDetailsHandler : IRequestHandler<GetProductDetailsQuery, RequestResult<GetProductDetailsResponseViewModel>>
    {
        private readonly IRepository<Product> _repository;

        public GetProductDetailsHandler(IRepository<Product> repository) => _repository = repository;

        public async Task<RequestResult<GetProductDetailsResponseViewModel>> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
        {
            var product =  _repository.GetByIDWithIncludes(request.Id, p => p
            .Include(p => p.Images)
            .Include(p=>p.Category));

            if (product == null)
            {
                return RequestResult<GetProductDetailsResponseViewModel>.Failure(ErrorCode.NotFound, "Product not found");
            }

            var response = new GetProductDetailsResponseViewModel
            {
                SKU = product.SKU,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                MinimumStock = product.MinimumStock,
                ImageUrls = product.Images.Where(i=>!i.Deleted).Select(i => i.Url).ToList(),
                CategoryName = product.Category.Name,
                UpdatedDate = product.UpdatedDate
            };

            return RequestResult<GetProductDetailsResponseViewModel>.Success(response, "Success");
        }
    }
}
