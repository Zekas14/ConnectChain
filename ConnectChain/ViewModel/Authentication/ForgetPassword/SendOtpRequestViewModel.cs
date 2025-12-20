namespace ConnectChain.ViewModel.Authentication.ForgetPassword
{
    public class SendOtpRequestViewModel
    {
        public string? Email { get; set; }
        public string? OTP { get; set; }
        public string? ResetToken { get; set; }
    }
}
