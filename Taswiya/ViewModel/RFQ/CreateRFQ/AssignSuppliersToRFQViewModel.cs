using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.RFQ.CreateRFQ
{
    public class AssignSuppliersToRFQViewModel
    {
        [Required]
        public int RfqId { get; set; }

        [Required]
        public List<string> SupplierIds { get; set; } = new();
    }
}
