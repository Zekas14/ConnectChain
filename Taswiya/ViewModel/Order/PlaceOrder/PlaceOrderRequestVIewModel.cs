using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Order.PlaceOrder
{
    public class PlaceOrderRequestVeiwModel
    {
        [Required]
        public string? SupplierId { get; set; }
        [Required]
        public string? PaymentMethod { get; set; }
        [MinLength(1)]
        public ICollection<OrderItems> Items { get; set; } = new List<OrderItems>();
        public string? Notes { get; set; }
        public  decimal SubTotal  => Items.Select(oi=>oi.Quantity*oi.UnitPrice).Sum();
        public decimal Discount { get ; set; }
        public string? FcmToken { get; set; }

    }
    public class OrderItems
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }   
        public decimal UnitPrice { get; set; }

    }
}
