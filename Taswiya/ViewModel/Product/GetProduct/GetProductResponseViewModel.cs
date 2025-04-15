namespace ConnectChain.ViewModel.Product.GetProduct
{
    public class GetProductResponseViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Stock { get; set; }

        public decimal Price { get; set; }
        public string? CategoryName { get; set; }
        public string? Image { get; set; } =string.Empty;   
    }
}
