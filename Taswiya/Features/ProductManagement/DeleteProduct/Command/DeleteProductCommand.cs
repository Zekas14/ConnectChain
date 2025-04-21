using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.ProductManagement.DeleteProduct.Command
{
    public record DeleteProductCommand(int ProductId) :IRequest<RequestResult<bool>>;
    public class DeleteProductCommandHandler(IRepository<Product> repository,IMediator mediator)
        : IRequestHandler<DeleteProductCommand, RequestResult< bool>>
    {
        private readonly IRepository<Product> _repository = repository;
        private readonly IMediator mediator = mediator;

        public async Task<RequestResult<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productExistResult = await mediator.Send(new IsProductExistQuery(request.ProductId));
            if (!productExistResult.isSuccess)
            {
                return RequestResult<bool>.Failure(productExistResult.errorCode, productExistResult.message);
            }
             _repository.Delete(productExistResult.data);
            await _repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true);
        }
    }

}
