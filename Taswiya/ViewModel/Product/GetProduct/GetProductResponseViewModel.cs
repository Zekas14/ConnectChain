namespace ConnectChain.ViewModel.Product.GetProduct
{
    public class GetProductResponseViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? CategoryName { get; set; }
        public List<string?> Images { get; set; } = new List<string?>();   
    }
}
