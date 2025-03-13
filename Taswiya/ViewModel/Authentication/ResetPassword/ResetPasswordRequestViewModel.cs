using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Authentication.ResetPassword
{
    public class ResetPasswordRequestViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}

