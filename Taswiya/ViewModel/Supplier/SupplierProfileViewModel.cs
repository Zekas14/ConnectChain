using ConnectChain.Models;
using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Supplier
{
    public class SupplierProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; init; }
        public string Address { get; init; }
        public string Email { get; set; }
        public ActivityCategory ActivityCategory { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
    }
}
