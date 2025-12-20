using ConnectChain.Models.Enums;

namespace ConnectChain.Models
{
    public class Order : BaseModel 
    {
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string SupplierId { get; set; }
        public Guid OrderNumber { get; set; }   
        public Supplier Supplier { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DeliveryFees { get; set; } = 0;
        public decimal TotalAmount { get => SubTotal + DeliveryFees - Discount;}
        public string PaymentMethod { get; set; } = "Cash";
        public string Notes { get; set; }
        public decimal Discount { get; set;}
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

}
