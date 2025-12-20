using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class ProductVariant : BaseModel
    {
        public string? Name { get; set;}
        public string? Type { get; set; }
        public decimal CustomPrice  { get; set; }
        public int  Stock  { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
