using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Supplier
{
    public class SupplierProfileUpdateViewModel
    {
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string? Name { get; set; }
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; init; }
        [StringLength(100, ErrorMessage = "Address cannot exceed 150 characters.")]
        public string? Address { get; init; }
        public int? ActivityCategoryID { get; init; }
        public List<int>? PaymentMethodsIDs { get; init; }
    }

}
