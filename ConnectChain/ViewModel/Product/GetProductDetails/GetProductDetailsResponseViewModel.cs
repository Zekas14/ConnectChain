namespace ConnectChain.ViewModel.Product.GetProductDetails
{
    public record GetProductDetailsResponseViewModel
    {
        public Guid SKU { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public int? MinimumStock { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public string? CategoryName { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
