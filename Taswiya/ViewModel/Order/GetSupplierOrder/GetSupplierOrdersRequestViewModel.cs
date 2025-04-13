using ConnectChain.Helpers;

namespace ConnectChain.ViewModel.Order.GetSupplierOrder
{
    public class GetSupplierOrdersRequestViewModel
    {
        
        public PaginationHelper pagination { get; set; }
        public string SupplierId { get; set; }
    }
}
