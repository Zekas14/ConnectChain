namespace ConnectChain.ViewModel.Product.GetCustomerProducts
{
    public class GetCustomerProductsResponseViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public string? CategoryName { get; set; }
        public string? Image { get; set; } = string.Empty;
        public bool IsStockAvailable { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierId { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty;
        public double SupplierRating { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsInWishlist { get; set; }
    }
}
