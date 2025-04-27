using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class Rate : BaseModel
    {
        public int RateNumber { get; set; }
        [ForeignKey("Supplier")]
        public string SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
