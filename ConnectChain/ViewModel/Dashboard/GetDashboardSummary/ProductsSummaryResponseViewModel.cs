namespace ConnectChain.ViewModel.Dashboard.GetDashboardSummary
{
    public class ProductsSummaryResponseViewModel
    {
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
    }
}
