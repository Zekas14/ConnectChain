using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.ProductVariant.AddProductVariant
{
    public class AddProductVariantRequestViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "Custom Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Custom Price must be non-negative")]
        public decimal CustomPrice { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be non-negative")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Product ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be a positive integer")]
        public int ProductId { get; set; }
    }
}
