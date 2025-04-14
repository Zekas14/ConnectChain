using Microsoft.AspNetCore.Identity;

namespace ConnectChain.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? BusinessType { get; set; }
        public string? Address { get; set; }

    }
}
