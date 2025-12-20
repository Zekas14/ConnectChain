using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.ViewModel.Product.AddProduct
{
    public class AddProductImageRequestViewModel
    {
        public IFormFile Image { get; set; }
        public int ProductId { get; set; }
    }
}
