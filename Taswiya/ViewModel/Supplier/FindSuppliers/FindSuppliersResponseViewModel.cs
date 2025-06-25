namespace ConnectChain.ViewModel.Supplier.FindSuppliers
{
    public class FindSuppliersResponseViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ActivityCategoryName { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public int TotalProducts { get; set; }
        public List<string> PaymentMethods { get; set; } = new();
    }
}
