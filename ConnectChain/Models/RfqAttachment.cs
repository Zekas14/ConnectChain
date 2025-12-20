using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class RfqAttachment : BaseModel
    {
        [ForeignKey("RFQ")]
        public int RfqId { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public RFQ RFQ { get; set; }
    }

}
