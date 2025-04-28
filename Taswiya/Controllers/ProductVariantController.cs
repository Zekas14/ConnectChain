using ConnectChain.Features.ProductManagement.ProductVariants.AddProductVariant.Commands;
using ConnectChain.Features.ProductManagement.ProductVariants.Common.Queries;
using ConnectChain.Filters;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.ProductVariant.AddProductVariant;
using ConnectChain.ViewModel.ProductVariant.GetProductVariant;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorization(Role.Supplier)]
    public class ProductVariantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductVariantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddProductVariant")]
        public async Task<ResponseViewModel<bool>> AddProductVariant(AddProductVariantRequestViewModel viewModel)
        {
            var result = await _mediator.Send(new AddProductVariantCommand
            {
                Name = viewModel.Name,
                Type = viewModel.Type,
                CustomPrice = viewModel.CustomPrice,
                Stock = viewModel.Stock,
                ProductId = viewModel.ProductId
            });
            return result.isSuccess
                ? new SuccessResponseViewModel<bool>(result.data, result.message)
                : new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }

        [HttpGet("GetProductVariants/{productId}")]
        public async Task<ResponseViewModel<IReadOnlyList<ProductVariantResponseViewModel>>> GetProductVariants(int productId)
        {
            var result = await _mediator.Send(new GetProductVariantsByProductIdQuery(productId));
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<ProductVariantResponseViewModel>>(result.data, "Product variants retrieved successfully")
                : new FailureResponseViewModel<IReadOnlyList<ProductVariantResponseViewModel>>(result.errorCode, result.message);
        }
    }
}