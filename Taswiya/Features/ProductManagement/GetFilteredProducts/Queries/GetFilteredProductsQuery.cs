using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.GetProduct;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace ConnectChain.Features.ProductManagement.GetFilteredProducts.Queries
{
    public record GetFilteredProductsQuery(int PageNumber, int PageSize, Dictionary<string, object> Filters) 
        : IRequest<RequestResult<IReadOnlyList<GetProductResponseViewModel>>>;

    public class GetFilteredProdcutsQueryHandler(IRepository<Product> repository):
        IRequestHandler<GetFilteredProductsQuery, RequestResult<IReadOnlyList<GetProductResponseViewModel>>>    {
        private readonly IRepository<Product> _repository = repository;

        public async Task<RequestResult<IReadOnlyList<GetProductResponseViewModel>>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
        {
            PaginationHelper pagination = new()
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
            var products = _repository.GetByPage(pagination);
            var parameter = Expression.Parameter(typeof(Product), "t");
            Expression expression = Expression.Constant(true);

            foreach (var filter in request.Filters)
            {
                var property = Expression.Property(parameter, filter.Key);
                var value = Expression.Constant(Convert.ChangeType(filter.Value, property.Type));

                var equalsExpression = Expression.Equal(property, value);
                expression = Expression.AndAlso(expression, equalsExpression);
            }

            var lambda = Expression.Lambda<Func<Product, bool>>(expression, parameter);

            var result = products.Where(lambda).Include(p => p.Supplier)
                .Select(t => new GetProductResponseViewModel
                {
                    Id = t.ID,
                    Name = t.Name,
                    Description = t.Description,
                    Price = t.Price,

                });
            if (!result.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetProductResponseViewModel>>.Success(result.ToList());
            }
            return RequestResult<IReadOnlyList<GetProductResponseViewModel>>.Failure(ErrorCode.NotFound, "No Products found ");

        }
    }
    }

