using ConnectChain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Product.AddProduct
{
    public class AddProductForSpecificSupplierRequestViewModel
    {
        [Required(ErrorMessage = "Product name is required")]
        public string? Name { get; set; }
        
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Minimum stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum stock must be 0 or greater")]
        public int MinimumStock { get; set; }
        
        [MaxLength(5, ErrorMessage = "Maximum 5 images are allowed.")]
        public List<IFormFile>? Images { get; set; } = [];
        
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be 0 or greater")]
        public int? Stock { get; set; }
        
        // CategoryId is fixed to 15 (Electronics) and SupplierId is fixed to the specific supplier
        // These are not included in the ViewModel as they are hardcoded in the command
    }
} 