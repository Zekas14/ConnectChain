using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    [Table("Reviews")]
    public class Review : BaseModel
    {
        public int Rate { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
