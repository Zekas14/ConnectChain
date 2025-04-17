namespace ConnectChain.ViewModel.Dashboard.GetTopSoldProducts
{
    public class GetTopSoldProductsResponseViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int TotalSoldQuantity { get; set; }
    }
}
