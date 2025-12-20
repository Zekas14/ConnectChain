using ConnectChain.Data.Context;
using ConnectChain.Features.SupplierManagement.Common.Queries;
using ConnectChain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.SupplierManagement.FcmToken.UpdateFcmToken.Commands
{
    public record UpdateFcmTokenCommand(string SupplierId , string FcmToken ): IRequest<RequestResult<bool>>;
    public class UpdateFcmTokenCommandHandler(ConnectChainDbContext context,IMediator mediator) : IRequestHandler<UpdateFcmTokenCommand, RequestResult<bool>>
    {
        private readonly ConnectChainDbContext context = context;
        private readonly IMediator mediator = mediator;

        public async Task<RequestResult<bool>> Handle(UpdateFcmTokenCommand request, CancellationToken cancellationToken)
        {
            var isSupplierExist = await mediator.Send(new IsSupplierExistsQuery(request.SupplierId),cancellationToken);
            if(!isSupplierExist.isSuccess)
            {
                return RequestResult<bool>.Failure(isSupplierExist.errorCode,isSupplierExist.message);
            }
            var rowsAffected =  context.Users.Where(s=>s.Id== request.SupplierId).ExecuteUpdate(s=>s.SetProperty(r=>r.FcmToken , request.FcmToken));
            if (rowsAffected == 1)
            {
                return RequestResult<bool>.Success(true, "Token Updated Successfully");
            }
            return RequestResult<bool>.Failure(ErrorCode.InternalServerError,"Update Failed");

        }
    }
}
