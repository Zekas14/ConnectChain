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

        [Required]
        public decimal QuotedPrice { get; set; }

        public int? DeliveryTimeInDays { get; set; }

        public string? Notes { get; set; }

        public DateTime? ValidUntil { get; set; }
        public QuotationStatus Status { get; set; } = QuotationStatus.Pending;
    }
}
