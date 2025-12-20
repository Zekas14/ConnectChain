using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Customer
{
    public class CustomerProfileUpdateViewModel
    {
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string? Name { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "Business type cannot exceed 50 characters.")]
        public string? BusinessType { get; set; }

        [Url(ErrorMessage = "Invalid image URL format.")]
        public string? ImageUrl { get; set; }
    }
}
