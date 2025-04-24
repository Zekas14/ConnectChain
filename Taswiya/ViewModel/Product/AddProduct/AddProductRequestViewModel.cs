using ConnectChain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Product.AddProduct
{
    public class AddProductRequestViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int MinimumStock { get; set; }
        [MaxLength(5, ErrorMessage = "Maximum 5 images are allowed.")]
        public List<IFormFile>? Images { get; set; } = [];
        public int? Stock { get; set; }
  //      public string? SupplierId { get; set; }
        public int CategoryId { get; set; }
    }
}
