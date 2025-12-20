namespace ConnectChain.Models
{
    public class UserPaymentMethod: BaseModel
    {
        
        public string UserID { get; set; }
        public int PaymentMethodID { get; set; }
        public User? Supplier { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
