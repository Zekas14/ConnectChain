namespace ConnectChain.ViewModel.ProductVariant.GetProductVariant
{
    public class ProductVariantResponseViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public decimal CustomPrice { get; set; }
        public int Stock { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
    }
}
