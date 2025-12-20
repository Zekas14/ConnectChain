using MediatR;
using ConnectChain.Data.Repositories.UserRepository;
using ConnectChain.Helpers;
using ConnectChain.ViewModel.Authentication.SignIn;

namespace ConnectChain.Features.AuthManagement.SignIn.Command
{
    public record UserSignInCommand(UserSignInRequestViewModel ViewModel) : IRequest<RequestResult<UserSignInResponseViewModel>>;
    public class UserSignInCommandHandler(IUserRepository userRepository) : IRequestHandler<UserSignInCommand, RequestResult<UserSignInResponseViewModel>>
    {
        private readonly IUserRepository userRepository = userRepository;

        public async Task<RequestResult<UserSignInResponseViewModel>> Handle(UserSignInCommand request, CancellationToken cancellationToken)
        {
            var result = await userRepository.SignIn(request.ViewModel);
            if (result.isSuccess)
            {
            return RequestResult<UserSignInResponseViewModel>.Success(result.data, "User Signed In Successfully");
            }
            return RequestResult<UserSignInResponseViewModel>.Failure(result.errorCode, result.message);
        }
    }
}
