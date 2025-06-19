using ConnectChain.Features.SupplierManagement.FindSuppliers.Queries;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Supplier.FindSuppliers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicSupplierController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("SearchByName")]
        public async Task<ResponseViewModel<IReadOnlyList<FindSuppliersResponseViewModel>>> SearchByName(
            [FromQuery] string searchName, 
            )
        {
            var result = await _mediator.Send(new SearchSuppliersByNameQuery(searchName);
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<FindSuppliersResponseViewModel>>(result.data, result.message)
                : new FailureResponseViewModel<IReadOnlyList<FindSuppliersResponseViewModel>>(result.errorCode, result.message);
        }

        [HttpGet("GetByBusinessType")]
        public async Task<ResponseViewModel<IReadOnlyList<FindSuppliersResponseViewModel>>> GetByBusinessType(
            [FromQuery] string businessType 
        )
        {
            var result = await _mediator.Send(new GetSuppliersByBusinessTypeQuery(businessType));
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<FindSuppliersResponseViewModel>>(result.data, result.message)
                : new FailureResponseViewModel<IReadOnlyList<FindSuppliersResponseViewModel>>(result.errorCode, result.message);
        }

        [HttpGet("FilterByRating")]
        public async Task<ResponseViewModel<IReadOnlyList<FindSuppliersResponseViewModel>>> FilterByRating(
            [FromQuery] double minRating = 0.0,
            [FromQuery] double maxRating = 5.0,
            [FromQuery] string? businessType = null)
        {
            var result = await _mediator.Send(new FilterSuppliersByRatingQuery(minRating, maxRating, businessType));
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<FindSuppliersResponseViewModel>>(result.data, result.message)
                : new FailureResponseViewModel<IReadOnlyList<FindSuppliersResponseViewModel>>(result.errorCode, result.message);
        }

        [HttpGet("Search")]
        public async Task<ResponseViewModel<IReadOnlyList<FindSuppliersResponseViewModel>>> Search(
            [FromQuery] string? searchName = null,
            [FromQuery] string? businessType = null,
            [FromQuery] double? minRating = null,
            [FromQuery] double? maxRating = null,
           )
        {
            var result = await _mediator.Send(new SearchSuppliersQuery(searchName, businessType, minRating, maxRating));
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<FindSuppliersResponseViewModel>>(result.data, result.message)
                : new FailureResponseViewModel<IReadOnlyList<FindSuppliersResponseViewModel>>(result.errorCode, result.message);
        }

     
}
