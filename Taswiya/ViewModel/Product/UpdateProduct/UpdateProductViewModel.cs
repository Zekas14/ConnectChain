namespace ConnectChain
{
    public class UpdateProductRequestViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public int CategoryId { get; set; }
    }
}