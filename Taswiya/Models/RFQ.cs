using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class RFQ : BaseModel
    {
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Unit { get; set; } = "Piece";

        public bool ShareBusinessCard { get; set; }

        public List<RfqAttachment> Attachments { get; set; } = new();

    }

}
