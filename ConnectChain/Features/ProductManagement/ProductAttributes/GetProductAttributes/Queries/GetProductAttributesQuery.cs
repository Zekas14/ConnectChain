using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.ProductAttribute.GetProductAttributes;
using MediatR;

namespace ConnectChain.Features.ProductManagement.ProductAttributes.GetProductAttributes.Queries
{
    public record GetProductAttributesByProductIdQuery(int ProductId) : IRequest<RequestResult<IReadOnlyList<ProductAttributeResponseViewModel>>>;

    public class GetProductAttributesByProductIdQueryHandler(
        IRepository<ProductAttribute> attributeRepository,
        IMediator mediator)
 : IRequestHandler<GetProductAttributesByProductIdQuery, RequestResult<IReadOnlyList<ProductAttributeResponseViewModel>>>
    {
        private readonly IRepository<ProductAttribute> _attributeRepository = attributeRepository;
        private readonly IMediator mediator = mediator;

        public async Task<RequestResult<IReadOnlyList<ProductAttributeResponseViewModel>>> Handle(GetProductAttributesByProductIdQuery request, CancellationToken cancellationToken)
        {
            var productResult = await mediator.Send(new IsProductExistQuery(request.ProductId),cancellationToken);
            if (!productResult.isSuccess)
            {
                return RequestResult<IReadOnlyList<ProductAttributeResponseViewModel>>.Failure(productResult.errorCode,productResult.message);
            }

            var attributes =  _attributeRepository.Get(attr => attr.ProductId == request.ProductId).
                Select(attr => new ProductAttributeResponseViewModel
            {
                Id = attr.ID,
                Key = attr.Key,
                Value = attr.Value,
                ProductId = attr.ProductId,
                ProductName = productResult.data.Name
            });

        

            return RequestResult<IReadOnlyList<ProductAttributeResponseViewModel>>.Success(attributes.ToList(),"Data Retrivied Successfully");
        }
    }
}
