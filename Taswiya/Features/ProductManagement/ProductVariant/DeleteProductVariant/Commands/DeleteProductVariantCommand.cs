using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using MediatR;

namespace ConnectChain.Features.ProductManagement.ProductVariant.DeleteProductVariant.Commands
{
    public record DeleteProductVariantCommand(int Id) : IRequest<RequestResult<bool>>;

    public class DeleteProductVariantCommandHandler : IRequestHandler<DeleteProductVariantCommand, RequestResult<bool>>
    {
        private readonly IRepository<ConnectChain.Models.ProductVariant> _variantRepository;

        public DeleteProductVariantCommandHandler(IRepository<ConnectChain.Models.ProductVariant> variantRepository)
        {
            _variantRepository = variantRepository;
        }

        public async Task<RequestResult<bool>> Handle(DeleteProductVariantCommand request, CancellationToken cancellationToken)
        {
            // Get the existing variant
            var variant = await _variantRepository.GetByIDAsync(request.Id);
            if (variant == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "The specified product variant does not exist.");
            }

            // Delete from repository and save
            _variantRepository.Delete(variant);
            await _variantRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Product Variant Deleted Successfully");
        }
    }
}
