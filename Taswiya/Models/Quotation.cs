using ConnectChain.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class Quotation : BaseModel
    {
        [ForeignKey("RFQ")]
        public int RfqId { get; set; }
        public RFQ RFQ { get; set; }

        [ForeignKey("Supplier")]
        public string SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("PaymentTerm")] 
        public int PaymentTermId { get; set; }

        public int Quantity { get; set; }
        public PaymentTerm PaymentTerm { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public int DeliveryTimeInDays { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Delivery fee must be a non-negative value.")]
        public double DeliveryFee { get; set; } = 0D;

        [Required]
        public string DeliveryTerm { get; set; } = string.Empty;

        public string? Notes { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Valid Until")]
        public DateTime ValidUntil { get; set; }
        public QuotationStatus Status { get; set; } = QuotationStatus.Pending;
    }
}
