using ConnectChain.Models.Enums;

namespace ConnectChain.ViewModel.Quotation
{
    public class GetQuotationViewModel
    {
        public int Id { get; set; }
        public int RfqId { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set;}
        public QuotationStatus Status { get; set; }
        public string categoryName { get; set; }
        public int Quantity { get; set; }
        public int PaymentTermId { get; set; }
        public decimal UnitPrice { get; set; }
        public int? DeliveryTimeInDays { get; set; }
        public string DeliveryTerm{ get; set; }
        public double DeliveryFee { get; set; }
        public string? Notes { get; set; }
        public DateTime? ValidUntil { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
