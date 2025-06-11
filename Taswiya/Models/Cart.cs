using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Models
{
    [Index(nameof(CustomerId))]
    public class Cart : BaseModel
    {
        public decimal SubTotal { get; set; }
        public List<CartItem> Items{ get; set; } = [];
        public string? CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
