using ConnectChain.Data.Context;
using ConnectChain.Data.Repositories.UserRepository;
using ConnectChain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController(ConnectChainDbContext context , UserManager<User> userManager)  : ControllerBase
    {
        private readonly ConnectChainDbContext context = context;
        private readonly UserManager<User> userManager = userManager;

        [HttpPost("Create")]
        public async Task<IActionResult> CreateSupplier( [FromBody] string id)
        {
            Supplier supplier = new Supplier {Id =id  };
           await userManager.CreateAsync(supplier);
            context.Add(supplier);
            context.SaveChanges();
            return Ok(new { message = "Supplier created successfully" });
        }

    }
}
