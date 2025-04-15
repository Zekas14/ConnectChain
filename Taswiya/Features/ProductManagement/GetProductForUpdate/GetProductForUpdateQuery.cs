using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.GetProductForUpdate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.ProductManagement.GetProductForUpdate
{
    public record GetProductForUpdateQuery(int ProductId) :IRequest<RequestResult<GetProductForUpdateResponseViewModel>>;
    public class GetProductForUpdateHandler(IRepository<Product> repository) : IRequestHandler<GetProductForUpdateQuery, RequestResult<GetProductForUpdateResponseViewModel>>
    {
        private readonly IRepository<Product> repository= repository;


        public async Task<RequestResult<GetProductForUpdateResponseViewModel>> Handle(GetProductForUpdateQuery request, CancellationToken cancellationToken)
        {
            var product =  repository.GetByIDWithIncludes(request.ProductId,p=>p.Include(p=>p.Images));
            if (product == null)
            {   
                return RequestResult<GetProductForUpdateResponseViewModel>.Failure(ErrorCode.NotFound, "Product not found");
            }

            var response = new GetProductForUpdateResponseViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                MinimumStock = product.MinimumStock,
                ImageUrls = product.Images.Select(i => i.Url).ToList(),
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId!
            };

            return RequestResult<GetProductForUpdateResponseViewModel>.Success(response,"Success");
        }
    }
}
