using ConnectChain.Helpers;
using ConnectChain.Models.Enums;

namespace ConnectChain.ViewModel.Order.GetSupplierOrder
{
    public class GetSupplierOrdersRequestViewModel
    {
        
        public OrderStatus? OrderStatus { get; set; } = null;
        public string SupplierId { get; set; }
    }
}
