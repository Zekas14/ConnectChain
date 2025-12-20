namespace ConnectChain.ViewModel.Order._ِAddOrder
{
    public class AddOrderRequestViewModel
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        
    }
}
