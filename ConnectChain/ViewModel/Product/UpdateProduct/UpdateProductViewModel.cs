using System.ComponentModel.DataAnnotations;

namespace ConnectChain
{
    public class UpdateProductRequestViewModel
    {
        [Required]
        public string Name { get; init; }
        [Required]
        public string? Description { get; init; }
        [Required]
        public decimal Price { get; init; }
        [Required]
        public int? Stock { get; init; }
        public List<IFormFile>? Images { get; init; }
        [Required]

        public int MinimumStock { get; init; }
        [Required]
        public int CategoryId { get; init; }
    }
}