using System;
using System.ComponentModel.DataAnnotations;
namespace ConnectChain.ViewModel.Quotation
{
    public class CreateQuotationViewModel
    {
        [Required]
        public int RfqId { get; set; }

        [Required]
        public string SupplierId { get; set; }

        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
       
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quoted price must be greater than zero.")]
        public decimal UnitPrice { get; set; }

        [Required]
        public int PaymentTermId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Delivery time must be at least 1 day.")]
        public int DeliveryTimeInDays { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Delivery fee must be a non-negative value.")]
        public double DeliveryFee { get; set; } = 0D;
        [Required]
        [StringLength(100, ErrorMessage = "Delivery term cannot exceed 100 characters.")]
        public string DeliveryTerm { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public DateTime ValidUntil { get; set; }
    }
}
