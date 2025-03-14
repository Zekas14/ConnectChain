namespace ConnectChain.ViewModel.Product.AddProduct
{
    public class AddProductRequestViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public string? SupplierId { get; set; }
        public int CategoryId { get; set; }
    }
}
