using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    
    public class Image : BaseModel
    {
        
        public string? Url { get; set; }
        [ForeignKey("Product")]
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
