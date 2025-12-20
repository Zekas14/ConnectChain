namespace ConnectChain.ViewModel.Order.GetOrderDetails
{
    public class GetOrderDetailsResponseViewModel
    {
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get => SubTotal + DeliveryFees - Discount; }
        public decimal DeliveryFees { get; set; }
        public decimal Discount { get; set; }
        public string Status { get; set; } 
        public IReadOnlyList<ProductsOrderedViewModel> Products { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string PaymentMethod { get; set; }

    }
}
