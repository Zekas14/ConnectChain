namespace ConnectChain.ViewModel.Cart.GetCartItems
{
    public class GetCartItemsResponseViewModel
    {
        public decimal Total { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new();
    }

    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int MinimumOrder { get; set; }
    }
}
