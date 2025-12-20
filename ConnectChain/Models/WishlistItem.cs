using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class WishlistItem : BaseModel
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }

        public Product Product { get; set; }
        public Customer Customer { get; set; }
    }
}
