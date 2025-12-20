using ConnectChain.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class RfqSupplierAssignment : BaseModel
    {
        [ForeignKey("RFQ")]
        public int RfqId { get; set; }
        public RFQ RFQ { get; set; }

        [ForeignKey("Supplier")]
        public string SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public RfqStatus Status { get; set; } = RfqStatus.Pending;
    }
}