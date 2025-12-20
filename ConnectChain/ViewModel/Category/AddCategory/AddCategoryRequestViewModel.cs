using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Category.AddCategory
{
    public class AddCategoryRequestViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
