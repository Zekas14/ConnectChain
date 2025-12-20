namespace ConnectChain.Models
{
    public class Customer : User
    {
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<RFQ> RFQs { get; set; } = [];
        public ICollection<WishlistItem> wishlist { get; set; }
        public Cart Cart { get; set; }
    }

}
