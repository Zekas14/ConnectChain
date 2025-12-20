using MediatR;
using ConnectChain.Data.Repositories.UserRepository;
using ConnectChain.Helpers;

namespace ConnectChain.Features.AuthManagement.ConfirmEmail.Command
{
    public record ConfirmEmailCommand(string UserId) : IRequest<RequestResult<bool>>;
    public class ConfirmEmailCommandHandler(IUserRepository userRepository) : IRequestHandler<ConfirmEmailCommand, RequestResult<bool>>
    {
        private readonly IUserRepository userRepository = userRepository;

        public async Task<RequestResult<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
           var requestResult = await  userRepository.ConfirmEmail(request.UserId);
            if (!requestResult.isSuccess)
            {
                return RequestResult<bool>.Failure(requestResult.errorCode, requestResult.message);
            }
            return RequestResult<bool>.Success(true, "Email Confirmed Successfully");
        }
    }
}
