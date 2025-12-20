namespace ConnectChain.ViewModel.ProductAttribute.GetProductAttributes
{
    public class ProductAttributeResponseViewModel
    {
        public int Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
    }
}
