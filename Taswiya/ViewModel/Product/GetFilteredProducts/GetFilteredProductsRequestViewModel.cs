namespace ConnectChain.ViewModel.Product.GetFilteredProducts
{
    public class GetFilteredProductsRequestViewModel
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public Dictionary<string, object> Filters { get;}
    }
}
