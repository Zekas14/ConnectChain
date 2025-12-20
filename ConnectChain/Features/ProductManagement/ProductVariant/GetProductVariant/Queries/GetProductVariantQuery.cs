using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.ViewModel.ProductVariant.GetProductVariant;
using MediatR;
using System.Collections.Generic;

namespace ConnectChain.Features.ProductManagement.ProductVariants.Common.Queries
{
    // Query to get product variants by product ID
    public record GetProductVariantsByProductIdQuery(int ProductId) : IRequest<RequestResult<IReadOnlyList<ProductVariantResponseViewModel>>>;

    public class GetProductVariantsByProductIdQueryHandler(
        IRepository<Models.ProductVariant> variantRepository,
        IMediator mediator) : IRequestHandler<GetProductVariantsByProductIdQuery, RequestResult<IReadOnlyList<ProductVariantResponseViewModel>>>
    {
        private readonly IRepository<Models.ProductVariant> _variantRepository = variantRepository;
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<IReadOnlyList<ProductVariantResponseViewModel>>> Handle(GetProductVariantsByProductIdQuery request, CancellationToken cancellationToken)
        {
            // Check if product exists
            var productExistResult = await _mediator.Send(new IsProductExistQuery(request.ProductId), cancellationToken);
            if (!productExistResult.isSuccess)
            {
                return RequestResult<IReadOnlyList<ProductVariantResponseViewModel>>.Failure(productExistResult.errorCode, productExistResult.message);
            }


            // Get variants for the product
            var variants = _variantRepository.Get(v => v.ProductId == request.ProductId);

            // Map to response view model
            var response = variants.Select(v => new ProductVariantResponseViewModel
            {
                Id = v.ID,
                Name = v.Name,
                Type = v.Type,
                CustomPrice = v.CustomPrice,
                Stock = v.Stock,
                ProductId = v.ProductId,
                ProductName = productExistResult.data.Name
            }).ToList();

            return RequestResult<IReadOnlyList<ProductVariantResponseViewModel>>.Success(response,"Product Retrived Successfully");
        }
    }
}