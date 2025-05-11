using System.Collections.Generic;

namespace Project.Domain.Entities
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<ProductSummaryViewModel> Products { get; set; }
    }
}