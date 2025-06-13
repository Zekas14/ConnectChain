using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Order.PlaceOrder
{
    public class PlaceOrderRequestViewModel
    {
        [Required]
        public string? PaymentMethod { get; set; }
        [MinLength(1)]
        public string? Notes { get; set; }
        public decimal Discount { get ; set; }

    }
    
}
