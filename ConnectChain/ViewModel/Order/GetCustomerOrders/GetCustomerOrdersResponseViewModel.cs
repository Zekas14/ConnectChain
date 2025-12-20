namespace ConnectChain.ViewModel.Order.GetCustomerOrders
{
    public class GetCustomerOrdersResponseViewModel
    {
        public Guid OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal DeliveryFees { get; set; }
        public decimal Discount { get; set; }
        public int TotalItems { get; set; }
        public List<CustomerOrderProductViewModel> Products { get; set; } = new();
    }

    public class CustomerOrderProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string ProductImage { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
    }
}
