using MediatR;
using ConnectChain.Data.Repositories.UserRepository;
using ConnectChain.Helpers;

namespace ConnectChain.Features.AuthManagement.ForgetPassword.Queries
{
    public record UserForgetPasswordQuery(string Email) : IRequest<RequestResult<bool>>;
    public class UserForgetPasswordQueryHandler(IUserRepository userRepository) : IRequestHandler<UserForgetPasswordQuery, RequestResult<bool>>
    {
        private readonly IUserRepository userRepository = userRepository;

        public async Task<RequestResult<bool>> Handle(UserForgetPasswordQuery request, CancellationToken cancellationToken)
        {
            var result = await userRepository.ForgetPassword(request.Email);
            if (result.isSuccess)
            {
                return RequestResult<bool>.Success(true, "Forget Password Email Sent , Please Check Your Email");
            }
            return RequestResult<bool>.Failure(ErrorCode.BadRequest, result.message);
        }
    }
}
