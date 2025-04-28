using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class ProductAttribute : BaseModel
    {
        public string? Key { get; set;}
        public string? Value { get; set;}
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
