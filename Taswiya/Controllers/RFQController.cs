using AutoMapper;
using ConnectChain.Features.RFQManagement.AssignSuppliersToRFQ.Commands;
using ConnectChain.Features.RFQManagement.CreateRFQ.commands;
using ConnectChain.Features.RFQManagement.GetRFQ.Queries;
using ConnectChain.Features.RFQManagement.RecommendedSuppliers.Query;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.RFQ.CreateRFQ;
using ConnectChain.ViewModel.RFQ.GetRecommendedSuppliers;
using ConnectChain.ViewModel.RFQ.GetRFQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RFQController(IMediator mediator, IMapper mapper) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpPost("Create")]
        [Authorization(Role.Customer)]
        public async Task<IActionResult> CreateRFQ([FromBody] CreateRFQViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new FailureResponseViewModel<int>(ErrorCode.BadRequest, errors));
            }
            var customerId = Request.GetIdFromToken();

            var command = new CreateRFQCommand(
                 customerId,
                 viewModel.ProductId,
                 viewModel.ProductName,
                 viewModel.CategoryId,
                 viewModel.Description,
                 viewModel.Quantity,
                 viewModel.Unit,
                 viewModel.Deadline,
                 viewModel.ShareBusinessCard,
                 viewModel.Attachments
             );

            var result = await _mediator.Send(command);
            if (result.isSuccess)
                return new SuccessResponseViewModel<int>(result.data, result.message);
            return new FailureResponseViewModel<int>(result.errorCode, result.message);
        }

        [HttpPost("AssignSuppliers")]
        [Authorization(Role.Customer)]
        public async Task<IActionResult> AssignSuppliers([FromBody] AssignSuppliersToRFQViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return new FailureResponseViewModel<bool>(ErrorCode.BadRequest, errors);
            }

            var command = new AssignSuppliersToRFQCommand(viewModel.RfqId, viewModel.SupplierIds);
            var result = await _mediator.Send(command);
            if (result.isSuccess)
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            return new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }

        [HttpGet("Customer/{rfqId:int}")]
        [Authorization(Role.Customer)]
        public async Task<IActionResult> GetCustomerRFQById(int rfqId)
        {
            var customerId = Request.GetIdFromToken();
            var result = await _mediator.Send(new GetCustomerRFQByIdQuery(rfqId, customerId));
            if (result.isSuccess)
                return new SuccessResponseViewModel<GetCustomerRFQByIdViewModel>(result.data, result.message);
            return new FailureResponseViewModel<GetCustomerRFQByIdViewModel>(result.errorCode, result.message);
        }

        [HttpGet("Supplier/{rfqId:int}")]
        [Authorization(Role.Supplier)]
        public async Task<IActionResult> GetSupplierRFQById(int rfqId)
        {
            var supplierId = Request.GetIdFromToken();
            var result = await _mediator.Send(new GetSupplierRFQByIdQuery(rfqId, supplierId));
            if (result.isSuccess)
                return new SuccessResponseViewModel<GetSupplierRFQByIdViewModel>(result.data, result.message);
            return new FailureResponseViewModel<GetSupplierRFQByIdViewModel>(result.errorCode, result.message);
        }

        [HttpGet("Customer")]
        [Authorization(Role.Customer)]
        public async Task<IActionResult> GetCustomerRFQs()
        {
            var customerId = Request.GetIdFromToken();
            var result = await _mediator.Send(new GetRFQsByCustomerQuery(customerId));
            if (result.isSuccess)
                return new SuccessResponseViewModel<List<GetCustomerRFQByIdViewModel>>(result.data, result.message);
            return new FailureResponseViewModel<List<GetCustomerRFQByIdViewModel>>(result.errorCode, result.message);
        }

        
        [HttpGet("Supplier")]
        [Authorization(Role.Supplier)]
        public async Task<IActionResult> GetSupplierRFQs()
        {
            var supplierId = Request.GetIdFromToken();
            var result = await _mediator.Send(new GetRFQsBySupplierQuery(supplierId));
            if (result.isSuccess)
                return new SuccessResponseViewModel<List<GetSupplierRFQByIdViewModel>>(result.data, result.message);
            return new FailureResponseViewModel<List<GetSupplierRFQByIdViewModel>>(result.errorCode, result.message);
        }

        [HttpGet("{rfqId:int}/RecommendedSuppliers")]
        [Authorization(Role.Customer)]
        public async Task<IActionResult> GetRecommendedSuppliers(int rfqId)
        {
            var result = await _mediator.Send(new RecommendSuppliersByProductCategoryQuery(rfqId));
            if (result.isSuccess)
                return new SuccessResponseViewModel<List<RecommendedSupplierViewModel>>(result.data, result.message);
            return new FailureResponseViewModel<List<RecommendedSupplierViewModel>>(result.errorCode, result.message);
        }

    }
}