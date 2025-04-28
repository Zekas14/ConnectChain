using ConnectChain.Features.ProductManagement.ProductAttributes.AddProductAttribute.Commands;
using ConnectChain.Features.ProductManagement.ProductAttributes.GetProductAttributes.Queries;
using ConnectChain.Filters;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.ProductAttribute.AddProductAttribute;
using ConnectChain.ViewModel.ProductAttribute.GetProductAttributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorization(Role.Supplier)]
    public class ProductAttributeController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("AddProductAttribute")]
        public async Task<ResponseViewModel<bool>> AddProductAttribute(AddProductAttributeRequestViewModel viewModel)
        {
            var result = await _mediator.Send(new AddProductAttributeCommand
            {
                Key = viewModel.Key,
                Value = viewModel.Value,
                ProductId = viewModel.ProductId
            });
            return result.isSuccess
                ? new SuccessResponseViewModel<bool>(result.data, result.message)
                : new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }

        [HttpGet("GetProductAttributes/{productId}")]
        public async Task<ResponseViewModel<IReadOnlyList<ProductAttributeResponseViewModel>>> GetProductAttributes(int productId)
        {
            var result = await _mediator.Send(new GetProductAttributesByProductIdQuery(productId));
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<ProductAttributeResponseViewModel>>(result.data, "Product attributes retrieved successfully")
                : new FailureResponseViewModel<IReadOnlyList<ProductAttributeResponseViewModel>>(result.errorCode, result.message);
        }
    }
}