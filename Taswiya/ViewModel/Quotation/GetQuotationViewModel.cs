namespace ConnectChain.ViewModel.Quotation
{
    public class GetQuotationViewModel
    {
        public int Id { get; set; }
        public int RfqId { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal QuotedPrice { get; set; }
        public int? DeliveryTimeInDays { get; set; }
        public string? Notes { get; set; }
        public DateTime? ValidUntil { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
