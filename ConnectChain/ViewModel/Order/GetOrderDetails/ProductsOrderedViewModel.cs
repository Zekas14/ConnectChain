namespace ConnectChain.ViewModel.Order.GetOrderDetails
{
    public class ProductsOrderedViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public List<string?> ImageUrl { get; set; } = [];

    }
}
