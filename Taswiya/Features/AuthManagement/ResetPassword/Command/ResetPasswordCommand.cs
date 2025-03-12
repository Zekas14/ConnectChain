using MediatR;
using ConnectChain.Data.Repositories.UserRepository;
using ConnectChain.Helpers;
using ConnectChain.ViewModel.Authentication.ResetPassword;

namespace ConnectChain.Features.AuthManagement.ResetPassword.Command
{
    public record ResetPasswordCommand(ResetPasswordRequestViewModel ViewModel) : IRequest<RequestResult<bool>>;
    public class ResetPasswordCommandHandler(IUserRepository userRepository) : IRequestHandler<ResetPasswordCommand, RequestResult<bool>>
    {
        private readonly IUserRepository userRepository = userRepository;

        public async Task<RequestResult<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await userRepository.ResetPassword(request.ViewModel);
            if (result.isSuccess)
            {
                return RequestResult<bool>.Success(true, "Password Reset Successfully");
            }
            return RequestResult<bool>.Failure(result.errorCode, result.message);
        }
    }
}
