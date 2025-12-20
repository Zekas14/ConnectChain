using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class UserShippingAddress: BaseModel
    {
        
        public string Address { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Phone { get; set; }
        [ForeignKey("User")]
        public string UserId {  get; set;}
        public User User { get; set; }
    }
}
