﻿using System.ComponentModel.DataAnnotations;

namespace ConnectChain.Helpers
{
    public class PaginationHelper
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNumber { get; set; } = 1;
        [Range(1, int.MaxValue, ErrorMessage = "Page size must be between 1 and 100.")]

        public int PageSize { get; set; } = 10;
    }
}
