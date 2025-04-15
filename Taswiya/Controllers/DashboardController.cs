using ConnectChain.Features.DashboardManagement.GetMonthlyStats.Queries;
using ConnectChain.Features.DashboardManagement.GetRevenueChart.Queries;
using ConnectChain.Features.DashboardManagement.GetTopSoldProducts.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Dashboard.GetMonthlyStats;
using ConnectChain.ViewModel.Dashboard.GetTopSoldProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpGet("GetRevenueChart")]
        public async Task<IActionResult> GetRevenueChart([FromQuery] string supplierId, [FromQuery] int? year = null)
        {
            int currentYear = year ?? DateTime.Now.Year; 
            var response = await mediator.Send(new GetRevenuePerMonthChartQuery(supplierId, currentYear));
            return response != null
                ? new SuccessResponseViewModel<Dictionary<Months, decimal>>(response, "Revenue chart retrieved successfully.")
                : new FailureResponseViewModel<Dictionary<Months, decimal>>(ErrorCode.NotFound, "No revenue data found.");
        }

        [HttpGet("GetMonthlyStats")]
        public async Task<IActionResult> GetMonthlyStats([FromQuery] string supplierId, [FromQuery] int? month = null, [FromQuery] int? year = null)
        {
            int currentMonth = month ?? DateTime.Now.Month; 
            int currentYear = year ?? DateTime.Now.Year;   

            var response = await mediator.Send(new GetMonthlyStatsQuery(supplierId, currentMonth, currentYear));
            return response != null
                ? new SuccessResponseViewModel<GetMonthlyStatsResponseViewModel>(response, "Monthly stats retrieved successfully.")
                : new FailureResponseViewModel<GetMonthlyStatsResponseViewModel>(ErrorCode.NotFound, "No monthly stats found.");
        }

        [HttpGet("GetTopSoldProducts")]
        public async Task<IActionResult> GetTopSoldProducts([FromQuery] string supplierId, [FromQuery] int? year = null, [FromQuery] int? month = null, [FromQuery] int limit = 5)
        {
            int currentYear = year ?? DateTime.Now.Year; 
            int currentMonth = month ?? DateTime.Now.Month; 

            var response = await mediator.Send(new GetTopSoldProductsQuery(supplierId, currentYear, currentMonth, limit));
            return response != null
                ? new SuccessResponseViewModel<IReadOnlyList<GetTopSoldProductsResponseViewModel>>(response, "Top sold products retrieved successfully.")
                : new FailureResponseViewModel<IReadOnlyList<GetTopSoldProductsResponseViewModel>>(ErrorCode.NotFound, "No top sold products found.");
        }
    }
}
