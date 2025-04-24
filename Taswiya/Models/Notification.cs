using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class Notification : BaseModel 
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        [ForeignKey("Supplier")]
        public string? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public bool IsRead { get; set; }
        public string? Type { get; set; }
    }
}
