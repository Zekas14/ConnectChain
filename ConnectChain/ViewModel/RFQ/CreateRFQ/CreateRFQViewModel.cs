using System.ComponentModel.DataAnnotations;

namespace ConnectChain.ViewModel.RFQ.CreateRFQ
{
    public class CreateRFQViewModel
    {
       
        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }

        public int? ProductId { get; set; }

        [Required]

        public int CategoryId { get; set; }


        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(50)]
        public string Unit { get; set; } = "Piece";

        public DateTime? Deadline { get; set; }

        public bool ShareBusinessCard { get; set; } = false;

        public List<IFormFile>? Attachments { get; set; }

    }
}
