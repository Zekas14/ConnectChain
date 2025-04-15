using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectChain.ViewModel.Product.GetFilteredProducts
{
    [NotMapped]
    public class GetFilteredProductsRequestViewModel
    {
        public Dictionary<string, object>? Filters { get;}
    }
}
