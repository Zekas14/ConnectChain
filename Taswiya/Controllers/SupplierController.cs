
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
using ConnectChain.Features.SupplierManagement.GetSupplierProfile.Query;
using ConnectChain.Features.SupplierManagement.UpdateSupplierProfile.Commands;
using ConnectChain.ViewModel;
using ConnectChain.Helpers;
using ConnectChain.Filters;
using ConnectChain.Models.Enums;

namespace ConnectChain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorization(roles : Role.Supplier)]
    public class SupplierController : ControllerBase
    {
        private readonly IMediator _mediator;
       

        public SupplierController(IMediator mediator , ConnectChainDbContext identityDbContext)
        {
            _mediator = mediator;
            
        }

        [HttpPut("updateProfile")]
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

    }

}
