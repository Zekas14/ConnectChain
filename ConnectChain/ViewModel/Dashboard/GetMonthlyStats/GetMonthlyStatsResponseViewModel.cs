namespace ConnectChain.ViewModel.Dashboard.GetMonthlyStats
{
    public class GetMonthlyStatsResponseViewModel
    {
        public int? TopSoldProductId { get; set; }
        public string? TopSoldProductName { get; set; } 
        public decimal AverageOrderTotal { get; set; }
        public decimal TotalRevenues { get; set; }
    }
}
