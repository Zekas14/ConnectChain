using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class Notification : BaseModel 
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        [ForeignKey("Supplier")]
        public string? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public bool IsRead { get; set; }
        public string? Type { get; set; }
    }
}
