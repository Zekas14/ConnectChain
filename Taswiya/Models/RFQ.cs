using ConnectChain.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class RFQ : BaseModel
    {
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Product")]
        public int? ProductId { get; set; } 
        public Product? Product { get; set; }
        public string ProductName { get; set; } = string.Empty;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Unit { get; set; } = "Piece";
        public DateTime? Deadline { get; set; }
        
        public bool ShareBusinessCard { get; set; }
        public List<RfqAttachment> Attachments { get; set; } = new();
        public RfqStatus Status { get; set; } = RfqStatus.Pending;
        public ICollection<RfqSupplierAssignment> SupplierAssignments { get; set; } = new List<RfqSupplierAssignment>();


    }

}
