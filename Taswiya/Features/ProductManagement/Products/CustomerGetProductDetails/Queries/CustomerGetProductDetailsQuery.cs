using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.CustomerGetProductDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.ProductManagement.Products.GetCustomerProductDetails.Queries
{
    public record CustomerGetProductDetailsQuery(int ProductId ) :IRequest<RequestResult<CustomerProductDetailsResponseViewModel>>;
    public class CustomerGetProductDetailsQueryHandler(IRepository<Product> repository) : IRequestHandler<CustomerGetProductDetailsQuery, RequestResult<CustomerProductDetailsResponseViewModel>>
    {
        private readonly IRepository<Product> _repository = repository;

        public async Task<RequestResult<CustomerProductDetailsResponseViewModel>> Handle(CustomerGetProductDetailsQuery request, CancellationToken cancellationToken)
        {
           var product = await _repository.Get(p => p.ID == request.ProductId)
                .Select(p=> new CustomerProductDetailsResponseViewModel
                {
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    MinimumStock = p.MinimumStock,
                    ImageUrls = p.Images.Where(i => !i.Deleted).Select(i => i.Url??"").ToList(),
                    CategoryName = p.Category != null ? p.Category.Name : null,
                    Sizes = p.ProductVariants
                        .Where(v => v.Type == "Size" && !v.Deleted)
                        .Select(v => v.Name)
                        .Distinct()
                        .ToList(),
                    Colors = p.ProductVariants
                        .Where(v => v.Type == "Color" && !v.Deleted)
                        .Select(v => v.Name)
                        .Distinct()
                        .ToList(),
                    IsStockAvailable = p.MinimumStock< p.Stock ,
                    AverageRating = p.Reviews.Where(r => !r.Deleted).Any() ? p.Reviews.Where(r => !r.Deleted).Average(r => r.Rate) : 0,
                    TotalRatings = p.Reviews.Count(r => !r.Deleted),
                    TotalReviews = p.Reviews.Count(r => !r.Deleted),
                    Reviews = p.Reviews.Where(r => !r.Deleted).Select(r => new ReviewsDto
                    {
                        Review= r.Body,
                        Rate = r.Rate,
                        CustomerName= r.Customer.Name,
                        CustomerImage= r.Customer.ImageUrl
                    }).ToList(),
                }).FirstOrDefaultAsync(cancellationToken);
            if(product is null)
            {
                return RequestResult<CustomerProductDetailsResponseViewModel>.Failure(ErrorCode.NotFound, "PRODUCT_NOT_FOUND");
            }
            return RequestResult<CustomerProductDetailsResponseViewModel>.Success(product, "Success");
        }
    }
}
