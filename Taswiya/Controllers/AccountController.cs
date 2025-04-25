using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using ConnectChain.Features.AuthManagement.ConfirmEmail.Command;
using ConnectChain.Features.AuthManagement.ForgetPassword.Queries;
using ConnectChain.Features.AuthManagement.Register.Command;
using ConnectChain.Features.AuthManagement.ResetPassword.Command;
using ConnectChain.Features.AuthManagement.SendConfirmationEmail.Queries;
using ConnectChain.Features.AuthManagement.SignIn.Command;
using ConnectChain.Features.AuthManagement.VerifyOtp.Command;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Authentication;
using ConnectChain.ViewModel.Authentication.ForgetPassword;
using ConnectChain.ViewModel.Authentication.ResetPassword;
using ConnectChain.ViewModel.Authentication.SignIn;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IMediator mediator) : ControllerBase//.BaseEndpoint<UserRegisterRequestViewModel, bool>
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromHeader] string fcmToken,UserRegisterRequestViewModel viewModel)
        {
            viewModel.FcmToken = fcmToken;
            
            var result = await _mediator.Send(new UserRegisterCommand(viewModel,Request.GenerateCallBackUrl));
            if (result.isSuccess)
            {
                return  new SuccessResponseViewModel<bool>(true,"Email Confirmation Sent , Please Confirm Your Email");
            }
                return new FailureResponseViewModel<bool>(ErrorCode.BadRequest, result.message);

        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(UserSignInRequestViewModel viewModel)
        {
            var result = await _mediator.Send(new UserSignInCommand(viewModel));
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<UserSignInResponseViewModel>(result.data, result.message);
            }
            return new FailureResponseViewModel<UserSignInResponseViewModel>(result.errorCode, result.message);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<ResponseViewModel<bool>> ConfirmEmail([FromQuery] string userId)
        {    
            var result = await _mediator.Send(new ConfirmEmailCommand(userId));
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            }
            return new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }
        [HttpPost("SendConfirmationEmail")]
        public async Task<IActionResult> SendConfirmationEmail(string email)
        {
            var result = await _mediator.Send(new SendConfirmationEmailQuery(email, Request.GenerateCallBackUrl));
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            }
            return new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var result = await _mediator.Send(new UserForgetPasswordQuery(email));
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            }
            
            return new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }
        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp(VerifyRequestOtpViewModel viewModel)
        {
            var result = await _mediator.Send(new VerifyOtpCommand(viewModel));
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            }
            return new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestViewModel viewModel)
        {
            var result = await _mediator.Send(new ResetPasswordCommand(viewModel));
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            }
            
            return new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }
    }
}
