using System.Collections.Generic;

namespace Project.Domain.Entities
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}