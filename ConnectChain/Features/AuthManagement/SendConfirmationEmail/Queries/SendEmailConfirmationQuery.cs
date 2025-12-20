using MediatR;
using ConnectChain.Data.Repositories.UserRepository;
using ConnectChain.Helpers;
using ConnectChain.Models;

namespace ConnectChain.Features.AuthManagement.SendConfirmationEmail.Queries
{
    public record SendConfirmationEmailQuery(string Email ,Func<string, string> generateUrl) : IRequest<RequestResult<bool>>;
    public class SendConfirmationEmailQueryHandler(IUserRepository userRepository) : IRequestHandler<SendConfirmationEmailQuery, RequestResult<bool>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<RequestResult<bool>> Handle(SendConfirmationEmailQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.SendConfirmationEmail(request.Email, request.generateUrl);
            if (!result.isSuccess)
            {
                return RequestResult<bool>.Failure(result.errorCode, result.message);
            }
            return RequestResult<bool>.Success(true, "Email Confirmation send , Please Verify your Email");
        }
    }
}
