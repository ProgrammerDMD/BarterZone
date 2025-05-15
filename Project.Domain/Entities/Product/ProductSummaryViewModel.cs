using System;
using System.Collections.Generic;

namespace Project.Domain.Entities
{
    public class ProductSummaryViewModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int CreatorId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}