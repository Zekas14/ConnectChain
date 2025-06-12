using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.Models
{
    public class Supplier : User

    {
        [ForeignKey("ActivityCategory")]
        public int? ActivityCategoryId { get; set; }
        public ActivityCategory? ActivityCategory { get; set; }
        public ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Order> Orders{ get; set; } = new List<Order>();
        public ICollection<Rate> Rate{ get; set; } = new List<Rate>();
    }
}
    