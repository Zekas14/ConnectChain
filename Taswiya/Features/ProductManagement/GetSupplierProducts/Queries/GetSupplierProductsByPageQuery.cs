using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.GetSupplierProduct;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.ProductManagement.GetSupplierProducts.Queries
{
    public record GetSupplierProductsByPageQuery(string SupplierId , PaginationHelper PaginationParams)
        : IRequest<RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>> ;

    public class GetSupplierProductsByPageQueryHandler(IRepository<Product> repository)

        : IRequestHandler<GetSupplierProductsByPageQuery, RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>>
    {
        private readonly IRepository<Product> repository = repository;

        public async Task<RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>> Handle(GetSupplierProductsByPageQuery request, CancellationToken cancellationToken)
        {
            var products = repository.GetAllByPage(request.PaginationParams)
                .Where(p => p.SupplierId == request.SupplierId)
                   .Include(p => p.Images)
                   .Include(p => p.Category)
                   
                   .Select(p => new GetSupplierProductResponseViewModel
                {
                    Id = p.ID,
                    Name = p.Name,
                    Stock = p.Stock,
                    Price = p.Price,
                    Image = p.Images.Where(i=>!i.Deleted).Select(x => x.Url).FirstOrDefault(),
                    CategoryName = p.Category!.Name
                });
            if (!products.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>.Success(products.ToList());
            }
            return RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>.Failure(ErrorCode.NotFound, "No Products found ");
        }
    }
}
