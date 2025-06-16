
﻿using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Models;
using ConnectChain.ViewModel.Supplier;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ConnectChain.Data.Context;
using ConnectChain.ViewModel;
using ConnectChain.Helpers;
using ConnectChain.Filters;
using ConnectChain.Models.Enums;
using ConnectChain.Features.SupplierManagement.Supplier.UpdateSupplierProfile.Commands;
using ConnectChain.Features.SupplierManagement.Supplier.GetSupplierProfile.Query;
using ConnectChain.Features.SupplierManagement.FcmToken.UpdateFcmToken.Commands;
using ConnectChain.Features.SupplierManagement.Supplier.GetSuppliers.Queries;
using ConnectChain.ViewModel.Supplier.GetSuppliers;

namespace ConnectChain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly IMediator _mediator;
       

        public SupplierController(IMediator mediator , ConnectChainDbContext identityDbContext)
        {
            _mediator = mediator;
            
        }

        [HttpPut("updateProfile")]
        [Authorization(roles: Role.Supplier)]

        public async Task<IActionResult> UpdateProfile( [FromBody] SupplierProfileUpdateViewModel model)
        {
            string? supplierId = Request.GetIdFromToken();
            var result = await _mediator.Send(new UpdateSupplierProfileCommand
            {
                Id = supplierId,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                ActivityCategoryID = model.ActivityCategoryID,
                PaymentMethodsIDs = model.PaymentMethodsIDs
            });

            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<bool>(result.data,result.message);
            }
            else
            {
                return new FailureResponseViewModel<bool>(result.errorCode, result.message);
            }
        }

        [HttpGet("GetProfile")]
        [Authorization(roles: Role.Supplier)]

        public async Task<IActionResult> GetProfile()
        {
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
            var userId = Request.GetIdFromToken();
           /* if (userId != supplierId)
            {
                return new FailureResponseViewModel<SupplierProfileViewModel>(ErrorCode.UnAuthorized, "unathorized");
            }*/
            var result = await _mediator.Send(new GetSupplierProfileQuery
            {
                 SupplierId = userId,
            });
            if (result.isSuccess)
            {
                return new SuccessResponseViewModel<SupplierProfileViewModel>(result.data, result.message);
            }
            else
            {
                return new FailureResponseViewModel<SupplierProfileViewModel>(result.errorCode, result.message);
            }
        }
        [HttpPut("UpdateFcmToken")]
        [Authorization(roles: Role.Supplier)]
        public async Task<ResponseViewModel<bool>> UpdateFcmToken([FromHeader] string fcmToken)
        {
            var supplierId = Request.GetIdFromToken();
            var result = await _mediator.Send(new UpdateFcmTokenCommand(supplierId,fcmToken));
            if (!result.isSuccess)
            {
            }
                return result.isSuccess ? new SuccessResponseViewModel<bool>(result.data, result.message):
                                 new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }
        [HttpGet("GetSuppliers")]
        [Authorization(Role.Customer)]
        public async Task<IActionResult> GetSuppliers()
        {
            var customerId = Request.GetIdFromToken();
            var response = await _mediator.Send(new GetSuppliersQuery(customerId!));
            if (response.isSuccess)
            {
                return new SuccessResponseViewModel<IReadOnlyList<GetSuppliersResponseViewModel>>(response.data,response.message);
            }
            return new FailureResponseViewModel<IReadOnlyList<GetSuppliersResponseViewModel>>(response.errorCode,response.message);
        }
    }

}
