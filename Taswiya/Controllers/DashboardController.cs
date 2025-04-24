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
using ConnectChain.Features.DashboardManagement.GetDashboardSummary.Queries;
using ConnectChain.ViewModel.Dashboard.GetDashboardSummary;
using ConnectChain.Filters;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorization(roles : Role.Supplier)]
    public class DashboardController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpGet("GetRevenueChart")]
        public async Task<IActionResult> GetRevenueChart( [FromQuery] int? year = null)
        {
            string? supplierId = Request.GetIdFromToken();
            var response = await mediator.Send(new GetRevenuePerMonthChartQuery(supplierId!, year ?? DateTime.Now.Year));
            return response != null
                ? new SuccessResponseViewModel<Dictionary<int, decimal>>(response, "Revenue chart retrieved successfully.")
                : new FailureResponseViewModel<Dictionary<int, decimal>>(ErrorCode.NotFound, "No revenue data found.");
        }

        [HttpGet("GetMonthlyStats")]
        public async Task<IActionResult> GetMonthlyStats([FromQuery] int? month = null, [FromQuery] int? year = null)
        {
            int currentMonth = month ?? DateTime.Now.Month; 
            int currentYear = year ?? DateTime.Now.Year;
            string? supplierId = Request.GetIdFromToken();

            var response = await mediator.Send(new GetMonthlyStatsQuery(supplierId!, currentMonth, currentYear));
            return response != null
                ? new SuccessResponseViewModel<GetMonthlyStatsResponseViewModel>(response, "Monthly stats retrieved successfully.")
                : new FailureResponseViewModel<GetMonthlyStatsResponseViewModel>(ErrorCode.NotFound, "No monthly stats found.");
        }

        [HttpGet("GetTopSoldProducts")]
        public async Task<IActionResult> GetTopSoldProducts([FromQuery] int? year = null, [FromQuery] int? month = null, [FromQuery] int limit = 5)
        {
            int currentYear = year ?? DateTime.Now.Year; 
            int currentMonth = month ?? DateTime.Now.Month;
            string? supplierId = Request.GetIdFromToken();

            var response = await mediator.Send(new GetTopSoldProductsQuery(supplierId!, currentYear, currentMonth, limit));
            return response != null
                ? new SuccessResponseViewModel<ICollection<GetTopSoldProductsResponseViewModel>>(response, "Top sold products retrieved successfully.")
                : new FailureResponseViewModel<ICollection<GetTopSoldProductsResponseViewModel>>(ErrorCode.NotFound, "No top sold products found.");
        }

        [HttpGet("GetOrdersSummary")]
        public async Task<IActionResult> GetOrdersSummary()
        {
            string? supplierId = Request.GetIdFromToken();
            var response = await mediator.Send(new GetOrdersSummaryQuery(supplierId!));
            return response != null
                ? new SuccessResponseViewModel<OrdersSummaryResponseViewModel>(response, "Orders summary retrieved successfully.")
                : new FailureResponseViewModel<OrdersSummaryResponseViewModel>(ErrorCode.NotFound, "No orders summary data found.");
        }

        [HttpGet("GetProductsSummary")]
        public async Task<IActionResult> GetProductsSummary()
        {
            string? supplierId = Request.GetIdFromToken();

            var response = await mediator.Send(new GetProductsSummaryQuery(supplierId!));
            return response != null
                ? new SuccessResponseViewModel<ProductsSummaryResponseViewModel>(response, "Products summary retrieved successfully.")
                : new FailureResponseViewModel<ProductsSummaryResponseViewModel>(ErrorCode.NotFound, "No products summary data found.");
        }
    }
}
