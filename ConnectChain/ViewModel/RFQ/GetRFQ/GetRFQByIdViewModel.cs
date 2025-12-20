using ConnectChain.Models.Enums;

namespace ConnectChain.ViewModel.RFQ.GetRFQ
{
    public class GetCustomerRFQByIdViewModel
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public DateTime? Deadline { get; set; }
        public bool ShareBusinessCard { get; set; }
        public RfqStatus Status { get; set; }
        public List<RfqAttachmentViewModel> Attachments { get; set; } = new();
        public List<RfqSupplierAssignmentViewModel> SupplierAssignments { get; set; } = new();
    }
   
    
        public class GetSupplierRFQByIdViewModel
        {
            public int Id { get; set; }
            public string ProductName { get; set; }
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string? Description { get; set; }
            public int Quantity { get; set; }
            public string Unit { get; set; }
            public DateTime? Deadline { get; set; }
            public bool ShareBusinessCard { get; set; }
            public RfqStatus Status { get; set; }
            public List<RfqAttachmentViewModel> Attachments { get; set; } = new();
            public RfqSupplierAssignmentViewModel? SupplierAssignment { get; set; }
        }
    
    public class RfqAttachmentViewModel
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }
    }

    public class RfqSupplierAssignmentViewModel
    {
        public int Id { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public RfqStatus Status { get; set; }
    }
}
