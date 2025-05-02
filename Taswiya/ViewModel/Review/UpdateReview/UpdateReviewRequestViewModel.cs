using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.Review.UpdateReview
{
    public class UpdateReviewRequestViewModel
    {
        [Required(ErrorMessage = "ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ID must be a positive integer")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Body is required")]
        [StringLength(1000, ErrorMessage = "Body cannot exceed 1000 characters")]
        public string Body { get; set; }

        [Required(ErrorMessage = "Rate is required")]
        [Range(1, 5, ErrorMessage = "Rate must be between 1 and 5")]
        public int Rate { get; set; }

        
    }
}
