namespace ConnectChain.Models
{
    public class CartItem : BaseModel
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Product Product { get; set; }
        public Cart Cart { get; set; }
    }
}
