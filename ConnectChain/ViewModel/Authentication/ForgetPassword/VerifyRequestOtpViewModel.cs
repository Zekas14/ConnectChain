using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Authentication.ForgetPassword
{
    public class VerifyRequestOtpViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? OTP { get; set; }
    }
}
