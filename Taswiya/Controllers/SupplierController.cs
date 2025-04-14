using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.SupplierManagment.UpdateSupplierProfile.Command;
using ConnectChain.Models;
using ConnectChain.ViewModel.Supplier;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ConnectChain.Data.Context;
using ConnectChain.Features.SupplierManagment.GetSupplierProfile.Query;

namespace ConnectChain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly IMediator _mediator;
       

        public SupplierController(IMediator mediator , ConnectChainDbContext identityDbContext)
        {
            _mediator = mediator;
            
        }

        [HttpPut("updateProfile {SupplierId}")]
        public async Task<IActionResult> UpdateProfile(string supplierId, [FromBody] SupplierProfileUpdateViewModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != supplierId)
            {
                return Unauthorized();
            }
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
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("getProfile/{SupplierId:alpha}")]
        public async Task<IActionResult> GetProfile(string supplierId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != supplierId)
            {
                return Unauthorized();
            }
            var result = await _mediator.Send(new GetSupplierProfileQuery
            {
                 SupplierId = supplierId,
            });
            return Ok(result);
        }

    }

}
