using ConnectChain.Features.QuotationManagement.ApproveQuotation;
using ConnectChain.Features.QuotationManagement.CreateQuotation.Commands;
using ConnectChain.Features.QuotationManagement.GetQuotation;
using ConnectChain.Features.QuotationManagement.RejectQuotation;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Quotation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuotationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuotationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        [Authorization(Role.Supplier)]
        public async Task<IActionResult> CreateQuotation([FromBody] CreateQuotationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return new FailureResponseViewModel<int>(ErrorCode.BadRequest, errors);
            }

            var command = new CreateQuotationCommand(
                viewModel.RfqId,
                viewModel.SupplierId,
                viewModel.QuotedPrice,
                viewModel.PaymentTermId,
                viewModel.DeliveryFee,
                viewModel.DeliveryTerm,
                viewModel.DeliveryTimeInDays,
                viewModel.Notes,
                viewModel.ValidUntil
            );

            var result = await _mediator.Send(command);
            if (result.isSuccess)
                return new SuccessResponseViewModel<int>(result.data, result.message);
            return new FailureResponseViewModel<int>(result.errorCode, result.message);
        }

        [HttpGet("{quotationId:int}")]
        [Authorization(Role.Supplier, Role.Customer)]
        public async Task<IActionResult> GetQuotationById(int quotationId)
        {
            var result = await _mediator.Send(new GetQuotationByIdQuery(quotationId));
            if (result.isSuccess)
                return new SuccessResponseViewModel<GetQuotationViewModel>(result.data, result.message);
            return new FailureResponseViewModel<GetQuotationViewModel>(result.errorCode, result.message);
        }

        [HttpGet("ByRFQ/{rfqId:int}")]
        [Authorization(Role.Customer)]
        public async Task<IActionResult> GetQuotationsByRFQ(int rfqId)
        {
            var result = await _mediator.Send(new GetQuotationsByRFQQuery(rfqId));
            if (result.isSuccess)
                return new SuccessResponseViewModel<List<GetQuotationViewModel>>(result.data, result.message);
            return new FailureResponseViewModel<List<GetQuotationViewModel>>(result.errorCode, result.message);
        }

        [HttpPost("Approve/{quotationId:int}")]
        [Authorization(Role.Customer)]
        public async Task<IActionResult> ApproveQuotation(int quotationId)
        {
            var customerId = Request.GetIdFromToken();
            var result = await _mediator.Send(new ApproveQuotationCommand(quotationId, customerId));
            if (result.isSuccess)
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            return new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }

        [HttpPost("Reject/{quotationId:int}")]
        [Authorization(Role.Customer)]
        public async Task<IActionResult> RejectQuotation(int quotationId)
        {
            var customerId = Request.GetIdFromToken();
            var result = await _mediator.Send(new RejectQuotationCommand(quotationId, customerId));
            if (result.isSuccess)
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            return new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }
    }
}