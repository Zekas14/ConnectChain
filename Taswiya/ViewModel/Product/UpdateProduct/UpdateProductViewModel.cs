namespace ConnectChain
{
    public class UpdateProductRequestViewModel
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public decimal Price { get; init; }
        public int? Stock { get; init; }
        public List<IFormFile>? Images { get; init; }
        public string[] RemainingImages { get; init; } = [];
        public int? MinimumStock { get; init; }
        public int CategoryId { get; init; }
    }
}