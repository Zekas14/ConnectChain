using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.ProductAttribute.AddProductAttribute
{
    public class AddProductAttributeRequestViewModel
    {
        [Required(ErrorMessage = "Key is required")]
        [StringLength(100, ErrorMessage = "Key cannot exceed 100 characters")]
        public string? Key { get; set; }

        [Required(ErrorMessage = "Value is required")]
        [StringLength(500, ErrorMessage = "Value cannot exceed 500 characters")]
        public string? Value { get; set; }

        [Required(ErrorMessage = "Product ID is required")]
        public int ProductId { get; set; }
    }
}
