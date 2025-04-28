using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class Product : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        [ForeignKey("Supplier")]
        public string? SupplierId { get; set; }
        public int MinimumStock { get; set; }
        public Guid SKU { get; set; }

        public Supplier? Supplier { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
      
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<ProductVariant> ProductVariants { get; set; }= new List<ProductVariant>();
        public ICollection<ProductAttribute> ProductAttributes { get; set; }= new List<ProductAttribute>();
        public ICollection<Review> Reviews { get; set; }= new List<Review>();
    }
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
    public class ProductAttribute : BaseModel
    {
        public string? Key { get; set;}
        public string? Value { get; set;}
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
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
