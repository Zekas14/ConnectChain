namespace ConnectChain.Models
{
    public class SupplierPaymentMethod
    {
        
        public int SupplierID { get; set; }
        public int PaymentMethodID { get; set; }
        public Supplier? Supplier { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
