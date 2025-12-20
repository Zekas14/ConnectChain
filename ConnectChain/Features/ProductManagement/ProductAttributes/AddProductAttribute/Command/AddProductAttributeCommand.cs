using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.ProductManagement.ProductAttributes.AddProductAttribute.Commands
{
    public record AddProductAttributeCommand : IRequest<RequestResult<bool>>
    {
        public string? Key { get; init; }
        public string? Value { get; init; }
        public int ProductId { get; init; }
    }

    public class AddProductAttributeCommandHandler : IRequestHandler<AddProductAttributeCommand, RequestResult<bool>>
    {
        private readonly IRepository<ProductAttribute> _repository;
        private readonly IMediator _mediator;

        public AddProductAttributeCommandHandler(
            IRepository<ProductAttribute> repository,
            IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<RequestResult<bool>> Handle(AddProductAttributeCommand request, CancellationToken cancellationToken)
        {
            // Validate if the product exists
            var productExistResult = await _mediator.Send(new IsProductExistQuery(request.ProductId), cancellationToken);
            if (!productExistResult.isSuccess)
            {
                return RequestResult<bool>.Failure(productExistResult.errorCode, productExistResult.message);
            }

            // Create new product attribute
            var productAttribute = new ProductAttribute
            {
                Key = request.Key,
                Value = request.Value,
                ProductId = request.ProductId
            };

            // Add to repository and save
            _repository.Add(productAttribute);
            await _repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Product Attribute Added Successfully");
        }
    }
}