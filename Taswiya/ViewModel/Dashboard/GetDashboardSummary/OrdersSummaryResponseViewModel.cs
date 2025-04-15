namespace ConnectChain.ViewModel.Dashboard.GetDashboardSummary
{
    public class OrdersSummaryResponseViewModel
    {
        public int CompletedOrders { get; set; }
        public int PendingOrders { get; set; }
        public int RejectedOrders { get; set; }
    }
}
