﻿using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
