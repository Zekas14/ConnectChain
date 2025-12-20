using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Authentication;
using ConnectChain.ViewModel.Authentication.ForgetPassword;
using ConnectChain.ViewModel.Authentication.ResetPassword;
using ConnectChain.ViewModel.Authentication.SignIn;

namespace ConnectChain.Data.Repositories.UserRepository
{
    public interface IUserRepository     {
        public Task<RequestResult<bool>> Register(UserRegisterRequestViewModel viewModel, Func<string, string> generateUrl);
        public Task<RequestResult<bool>> SendConfirmationEmail(string email, Func<string, string> generateUrl);
        public Task<RequestResult<bool>> ConfirmEmail(string userId);
        public Task<RequestResult<bool>> ForgetPassword(string email);
        public Task<RequestResult<bool>> SendOtp(SendOtpRequestViewModel viewModel);
        public  Task<RequestResult<bool>> VerifyOtp(VerifyRequestOtpViewModel viewModel);
        public Task<RequestResult<bool>> ResetPassword(ResetPasswordRequestViewModel viewModel);
        public Task<RequestResult<UserSignInResponseViewModel>> SignIn(UserSignInRequestViewModel viewModel);
    }

}
