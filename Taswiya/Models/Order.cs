using ConnectChain.Models.Enums;

namespace ConnectChain.Models
{
    public class Order : BaseModel 
    {
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

}
