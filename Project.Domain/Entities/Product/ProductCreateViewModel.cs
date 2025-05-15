using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Project.Domain.Entities
{
    public class ProductCreateViewModel
    {
        [Required(ErrorMessage = "Product title is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Title must be between 6 and 50 characters.")]
        public string Title { get; set; }
        
        public HttpPostedFileBase Image { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Category")]
        public int Category { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1024, MinimumLength = 30, ErrorMessage = "Description must be between 30 and 1024 characters.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}