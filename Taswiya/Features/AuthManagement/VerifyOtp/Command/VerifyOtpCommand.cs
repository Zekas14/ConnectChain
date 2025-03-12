using MediatR;
using ConnectChain.Data.Repositories.UserRepository;
using ConnectChain.Helpers;
using ConnectChain.ViewModel.Authentication.ForgetPassword;

namespace ConnectChain.Features.AuthManagement.VerifyOtp.Command
{
    public record VerifyOtpCommand(VerifyRequestOtpViewModel ViewModel) : IRequest<RequestResult<bool>>;
    public class VerifyOtpCommandHandler(IUserRepository userRepository) : IRequestHandler<VerifyOtpCommand, RequestResult<bool>>
    {
        private readonly IUserRepository userRepository = userRepository;

        public async Task<RequestResult<bool>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var result = await userRepository.VerifyOtp(request.ViewModel);
            if (result.isSuccess)
            {
                return RequestResult<bool>.Success(true, "OTP Verified Successfully");
            }
            return RequestResult<bool>.Failure(result.errorCode, result.message);
        }
    }
}
