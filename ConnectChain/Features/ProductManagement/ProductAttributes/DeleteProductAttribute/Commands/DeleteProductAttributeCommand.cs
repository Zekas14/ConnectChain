using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.ProductManagement.ProductAttributes.DeleteProductAttribute.Commands
{
    public record DeleteProductAttributeCommand(int Id) : IRequest<RequestResult<bool>>;

    public class DeleteProductAttributeCommandHandler : IRequestHandler<DeleteProductAttributeCommand, RequestResult<bool>>
    {
        private readonly IRepository<ProductAttribute> _attributeRepository;

        public DeleteProductAttributeCommandHandler(IRepository<ProductAttribute> attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }

        public async Task<RequestResult<bool>> Handle(DeleteProductAttributeCommand request, CancellationToken cancellationToken)
        {
            // Get the existing attribute
            var attribute = await _attributeRepository.GetByIDAsync(request.Id);
            if (attribute == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "The specified product attribute does not exist.");
            }

            // Delete from repository and save
            _attributeRepository.Delete(attribute);
            await _attributeRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Product Attribute Deleted Successfully");
        }
    }
}
