namespace ConnectChain.Models
{
    public class Supplier : User

    {
     
        public ICollection<Order> Orders{ get; set; } = new List<Order>();
    }
}
    