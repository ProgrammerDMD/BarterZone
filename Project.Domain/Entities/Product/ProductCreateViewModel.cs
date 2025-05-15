using System.Web;

namespace Project.Domain.Entities
{
    public class ProductCreateViewModel
    {
        public string Title { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public double Price { get; set; }
        public int Category { get; set; }
        public string Description { get; set; }
    }
}