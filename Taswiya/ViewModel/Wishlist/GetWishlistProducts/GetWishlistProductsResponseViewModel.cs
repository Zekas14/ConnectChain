namespace ConnectChain.ViewModel.Wishlist.GetWishlistProducts
{
    public class GetWishlistProductsResponseViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public string? CategoryName { get; set; }
        public string? Image { get; set; } = string.Empty;
        public bool IsStockAvailable { get; set; }
        public DateTime AddedToWishlistDate { get; set; }
    }
}
