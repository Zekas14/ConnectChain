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
        public async Task<ResponseViewModel<bool> > Register(UserRegisterRequestViewModel viewModel)
        {
            
            if (!ModelState.IsValid)
            {
                
                var errors =string.Join("," ,ModelState.Select(e => e.Value!.Errors));
                return new FaluireResponseViewModel<bool>(ErrorCode.InvalidInput,errors) ;
            }
            var result = await _mediator.Send(new UserRegisterCommand(viewModel,Request.GenerateCallBackUrl));
            if (result.isSuccess)
            {
                return  new SuccessResponseViewModel<bool>(true,"Email Confirmation Sent , Please Confirm Your Email");
            }
                return new FaluireResponseViewModel<bool>(ErrorCode.BadRequest, result.message);

        }
        [HttpPost("SignIn")]
        public async Task<ResponseViewModel<UserSignInResponseViewModel>> SignIn(UserSignInRequestViewModel viewModel)
        {
            var result = await _mediator.Send(new UserSignInCommand(viewModel));
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<UserSignInResponseViewModel>(result.data, result.message);
            }
            return new FaluireResponseViewModel<UserSignInResponseViewModel>(result.errorCode, result.message);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<ResponseViewModel<bool>> ConfirmEmail([FromQuery] string userId)
        {    
            var result = await _mediator.Send(new ConfirmEmailCommand(userId));
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            }
            return new FaluireResponseViewModel<bool>(result.errorCode, result.message);
        }
        [HttpGet("SendConfirmationEmail")]
        public async Task<ResponseViewModel<bool>> SendConfirmationEmail(string email)
        {
            var result = await _mediator.Send(new SendConfirmationEmailQuery(email, Request.GenerateCallBackUrl));
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            }
            return new FaluireResponseViewModel<bool>(result.errorCode, result.message);
        }
        [HttpPost("ForgetPassword")]
        public async Task<ResponseViewModel<bool>> ForgetPassword(string email)
        {
            var result = await _mediator.Send(new UserForgetPasswordQuery(email));
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            }
            return new FaluireResponseViewModel<bool>(result.errorCode, result.message);
        }
        [HttpPost("VerifyOtp")]
        public async Task<ResponseViewModel<bool>> VerifyOtp(VerifyRequestOtpViewModel viewModel)
        {
            var result = await _mediator.Send(new VerifyOtpCommand(viewModel));
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            }
            return new FaluireResponseViewModel<bool>(result.errorCode, result.message);
        }
        [HttpPut("ResetPassword")]
        public async Task<ResponseViewModel<bool>> ResetPassword(ResetPasswordRequestViewModel viewModel)
        {
            var result = await _mediator.Send(new ResetPasswordCommand(viewModel));
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            }
            
            return new FaluireResponseViewModel<bool>(result.errorCode, result.message);
        }
    }
}
