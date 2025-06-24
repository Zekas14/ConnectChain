using ConnectChain.Features.NotificationManagement.GetNotification.Queries;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Notification.GetNotification;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;
        [HttpGet("GetSupplierNotifications")]
        [Authorization(Role.Supplier)]
        public async Task<ResponseViewModel<IReadOnlyList<GetNotificationResponseViewModel>>> GetSupplierNotifications()
        {
            var supplierId = Request.GetIdFromToken();
            var response = await mediator.Send(new GetNotificationQuery(supplierId!));
            return response.isSuccess ?
                new SuccessResponseViewModel<IReadOnlyList<GetNotificationResponseViewModel>>(response.data, response.message) :
                new FailureResponseViewModel<IReadOnlyList<GetNotificationResponseViewModel>>(response.errorCode,response.message);
        }

     
        

        }
}
