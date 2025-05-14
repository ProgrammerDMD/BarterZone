using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Project.BusinessLogic.Core;

namespace Project.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductService _productService = new ProductService();
        
        // Categories
        // Key = The ID of the category
        // Value = Text to Display
        private static readonly Dictionary<string, string> categories = new Dictionary<string, string>
        {
            ["electronics"] = "Electronics",
            ["clothing_fashion"] = "Clothing & Fashion",
            ["collectibles"] = "Collectibles",
            ["books-media"] = "Books & Media",
            ["home-furniture"] = "Home & Furniture",
            ["toys-games"] = "Toys & Games",
            ["sports-outdoors"] = "Sports & Outdoors",
            ["automotive"] = "Automotive",
            ["beauty-health"] = "Beauty & Health",
            ["tools-diy"] = "Tools & DIY",
            ["musical-instruments"] = "Musical Instruments",
            ["pet-supplies"] = "Pet Supplies",
            ["office-school-supplies"] = "Office & School Supplies",
            ["tickets-experiences"] = "Tickets & Experiences"
        };

        public async Task<ActionResult> Index(int? page)
        {
            ViewData["categories"] = categories;
            int currentPage = page ?? 1;
            
            var products = await _productService.GetProductsByPage(currentPage - 1, 1);
            return View(products);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}