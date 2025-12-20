using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Product.SearchProduct
{
    public class SearchProductRequestViewModel
    {
        [Required]
        public string SupplierId { get; set; }
        [Required]
        public string SearchKey { get; set; }
    }
}
