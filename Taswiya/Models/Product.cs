using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class Product : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        [ForeignKey("Supplier")]
        public string? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
