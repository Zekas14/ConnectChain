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
        [Range(0.01, double.MaxValue, ErrorMessage = "Quoted price must be greater than zero.")]
        public decimal QuotedPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Delivery time must be at least 1 day.")]
        public int? DeliveryTimeInDays { get; set; }

        public string? Notes { get; set; }

        public DateTime? ValidUntil { get; set; }
    }
}
