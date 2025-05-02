using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Review.AddReview
{
    public class AddReviewRequestViewModel
    {
        [Required(ErrorMessage = "Body is required")]
        [StringLength(1000, ErrorMessage = "Body cannot exceed 1000 characters")]
        public string Body { get; set; }

        [Required(ErrorMessage = "Rate is required")]
        [Range(1, 5, ErrorMessage = "Rate must be between 1 and 5")]
        public int Rate { get; set; }

        [Required(ErrorMessage = "Product ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be a positive integer")]
        public int ProductId { get; set; }

    }
}
