namespace ConnectChain.ViewModel.Dashboard.GetTopSoldProducts
{
    public class GetTopSoldProductsResponseViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public int TotalSoldQuantity { get; set; }
    }
}
