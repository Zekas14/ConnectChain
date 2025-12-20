using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.ProductManagement.ProductVariants.AddProductVariant.Commands
{
    public record AddProductVariantCommand : IRequest<RequestResult<bool>>
    {
        public string? Name { get; init; }
        public string? Type { get; init; }
        public decimal CustomPrice { get; init; }
        public int Stock { get; init; }
        public int ProductId { get; init; }
    }

    public class AddProductVariantCommandHandler : IRequestHandler<AddProductVariantCommand, RequestResult<bool>>
    {
        private readonly IRepository<Models.ProductVariant> _variantRepository;
        private readonly IMediator _mediator;

        public AddProductVariantCommandHandler(
            IRepository<Models.ProductVariant> variantRepository,
            IMediator mediator)
        {
            _variantRepository = variantRepository;
            _mediator = mediator;
        }

        public async Task<RequestResult<bool>> Handle(AddProductVariantCommand request, CancellationToken cancellationToken)
        {
            var productExistResult = await _mediator.Send(new IsProductExistQuery(request.ProductId), cancellationToken);
            if (!productExistResult.isSuccess)
            {
                return RequestResult<bool>.Failure(productExistResult.errorCode, productExistResult.message);
            }

            var productVariant = new Models.ProductVariant
            {
                Name = request.Name,
                Type = request.Type,
                CustomPrice = request.CustomPrice,
                Stock = request.Stock,
                ProductId = request.ProductId
            };

            _variantRepository.Add(productVariant);
            await _variantRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Product Variant Added Successfully");
        }
    }
}   