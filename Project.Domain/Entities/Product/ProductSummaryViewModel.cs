using System;
using System.Collections.Generic;

namespace Project.Domain.Entities
{
    public class ProductSummaryViewModel
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}