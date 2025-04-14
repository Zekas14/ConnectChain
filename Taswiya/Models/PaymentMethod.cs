using System.Collections.Generic;

namespace ConnectChain.Models
{
    public class PaymentMethod : BaseModel
    {
        
        public string Name { get; set; } = string.Empty;
        public IEnumerable<SupplierPaymentMethod> SupplierPaymentMethods { get; set; } = new List<SupplierPaymentMethod>();
        public IEnumerable<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }
}
