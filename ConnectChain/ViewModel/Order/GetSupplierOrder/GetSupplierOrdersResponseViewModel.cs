namespace ConnectChain.ViewModel.Order.GetSupplierOrder
{
    public class GetSupplierOrdersResponseViewModel
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public IReadOnlyList<string?> Products { get; set; } = new List<string?>();

    }
}