using Microsoft.AspNetCore.Identity;

namespace ConnectChain.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? BusinessType { get; set; }
        public string? Address { get; set; } 
        public string? ImageUrl { get; set; }
        public string? FcmToken { get;set; }   
        public IEnumerable<UserPaymentMethod> UserPaymentMethods { get; set; } = [];
        public ICollection<UserShippingAddress> UserShippingAddresses { get; } = [];
    }
}
