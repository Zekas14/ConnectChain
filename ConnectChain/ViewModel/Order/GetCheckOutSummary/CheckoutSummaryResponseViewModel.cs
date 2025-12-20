namespace ConnectChain.ViewModel.Order.GetCheckOutSummary
{
    public class CheckoutSummaryResponseViewModel
    {
      public List<GetShippingAddressesResponseViewModel> ShippingAddress { get; set; } = default!;
        public List<CartItemViewModel> Items { get; set; } = [];
        public decimal SubTotal { get; set; }
        public decimal ShippingFees { get; set; }
        public decimal Total => SubTotal + ShippingFees;
    }
    public class GetShippingAddressesResponseViewModel
    {
        public string Address { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Phone { get; set; }

    }
    public class CartItemViewModel
    {
        public string ProductName { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
    }
}
