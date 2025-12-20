using MediatR;
using ConnectChain.Data.Repositories.UserRepository;
using ConnectChain.Helpers;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Authentication;

namespace ConnectChain.Features.AuthManagement.Register.Command
{
    public record UserRegisterCommand(UserRegisterRequestViewModel ViewModel,Func<string , string> Url) : IRequest<RequestResult<bool>>
    {
    }
    public class UserRegisterCommandHandler(IUserRepository userRepository) : IRequestHandler<UserRegisterCommand, RequestResult<bool>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public Task<RequestResult<bool>> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            return _userRepository.Register(request.ViewModel,request.Url);
        }

    }
}
