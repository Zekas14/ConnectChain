using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.GetSupplierProduct;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace ConnectChain.Features.ProductManagement.Products.GetFilteredProducts.Queries
{
    public record GetFilteredProductsQuery(Dictionary<string, object> Filters)
        : IRequest<RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>>;

    public class GetFilteredProdcutsQueryHandler(IRepository<Product> repository) :
        IRequestHandler<GetFilteredProductsQuery, RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>>
    {
        private readonly IRepository<Product> _repository = repository;

        public async Task<RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
        {

            var products = _repository.GetAll();
            var parameter = Expression.Parameter(typeof(Product), "t");
            Expression expression = Expression.Constant(true);
            if (request.Filters == null || !request.Filters.Any())
            {
                var data = products.Include(p => p.Supplier)
                    .Select(t => new GetSupplierProductResponseViewModel
                    {
                        Id = t.ID,
                        Name = t.Name,
                        Stock = t.Stock,
                        Price = t.Price,
                        CategoryName = t.Category!.Name,
                        Image = t.Images.Select(x => x.Url).FirstOrDefault(),
                    });
                if (!data.IsNullOrEmpty())
                {
                    return RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>.Success(data.ToList());
                }
                return RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>.Failure(ErrorCode.NotFound, "No Products found ");
            }
            foreach (var filter in request.Filters)
            {
                var property = Expression.Property(parameter, filter.Key);
                var value = Expression.Constant(Convert.ChangeType(filter.Value, property.Type));

                var equalsExpression = Expression.Equal(property, value);
                expression = Expression.AndAlso(expression, equalsExpression);
            }

            var lambda = Expression.Lambda<Func<Product, bool>>(expression, parameter);

            var result = products.Where(lambda).Include(p => p.Supplier)
                .Select(t => new GetSupplierProductResponseViewModel
                {
                    Id = t.ID,
                    Name = t.Name,
                    Stock = t.Stock,
                    Price = t.Price,
                    CategoryName = t.Category!.Name,
                    Image = t.Images.Select(x => x.Url).FirstOrDefault(),
                });
            if (!result.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>.Success(result.ToList());
            }
            return RequestResult<IReadOnlyList<GetSupplierProductResponseViewModel>>.Failure(ErrorCode.NotFound, "No Products found ");

        }
    }
}

