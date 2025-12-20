namespace ConnectChain.ViewModel.Product.GetProductForUpdate
{
    public class GetProductForUpdateResponseViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public int MinimumStock { get; set; }
        public Dictionary<int,string> ImageUrls { get; set; } = [];
        public int CategoryId { get; set; }
        public string SupplierId { get; set; }
    }

}
