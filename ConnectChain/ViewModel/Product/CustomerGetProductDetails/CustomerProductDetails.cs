namespace ConnectChain.ViewModel.Product.CustomerGetProductDetails
{
    public class CustomerProductDetailsResponseViewModel
    {
        public string SKU { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public int? MinimumStock { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public string? CategoryName { get; set; }

        public ICollection<string> Sizes { get; set; } = new List<string>();
        public ICollection<string> Colors { get; set; } = new List<string>();
        public ICollection<ReviewsDto> Reviews { get; set; } = new List<ReviewsDto>();
       // public Dictionary<string, string> Attributes { get; set; } = new();

        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public int TotalReviews { get; set; }

       // public bool IsOnSale { get; set; }        
        public bool IsStockAvailable { get; set; } 
    }
    public class ReviewsDto
    {
        public string Review { get; set; }
        public decimal Rate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerImage { get; set; }
    }
}
