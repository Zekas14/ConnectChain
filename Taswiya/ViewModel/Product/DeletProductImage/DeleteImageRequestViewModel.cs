using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Product.DeletProductImage
{
    public class DeleteImageRequestViewModel
    {
        [Required(ErrorMessage ="Image Url is Required")]
        public string ImageUrl { get; set; }
    }
}
