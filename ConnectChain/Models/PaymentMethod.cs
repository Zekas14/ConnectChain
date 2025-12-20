using System.Collections.Generic;

namespace ConnectChain.Models
{
    public class PaymentMethod : BaseModel
    {
        
        public string Name { get; set; } = string.Empty;
        public IEnumerable<UserPaymentMethod> UserPaymentMethods { get; set; } = [];
    }
}
