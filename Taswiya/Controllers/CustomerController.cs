using ConnectChain.Features.CustomerManagement.GetCustomerProfile.Queries;
using ConnectChain.Features.CustomerManagement.UpdateCustomerProfile.Commands;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Customer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorization(Role.Customer)]
    public class CustomerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("GetProfile")]
        public async Task<ResponseViewModel<CustomerProfileViewModel>> GetProfile()
        {
            string? customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<CustomerProfileViewModel>(ErrorCode.UnAuthorized, "Unauthorized");
            }

            var result = await _mediator.Send(new GetCustomerProfileQuery(customerId));
            return result.isSuccess
                ? new SuccessResponseViewModel<CustomerProfileViewModel>(result.data, result.message)
                : new FailureResponseViewModel<CustomerProfileViewModel>(result.errorCode, result.message);
        }

        [HttpPut("UpdateProfile")]
        public async Task<ResponseViewModel<bool>> UpdateProfile([FromBody] CustomerProfileUpdateViewModel model)
        {
            string? customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<bool>(ErrorCode.UnAuthorized, "Unauthorized");
            }

            var result = await _mediator.Send(new UpdateCustomerProfileCommand
            {
                CustomerId = customerId,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                BusinessType = model.BusinessType,
                ImageUrl = model.ImageUrl
            });

            return result.isSuccess
                ? new SuccessResponseViewModel<bool>(result.data, result.message)
                : new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }

        
    }
}
