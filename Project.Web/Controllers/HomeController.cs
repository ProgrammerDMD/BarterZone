using System.Collections.Generic;
using System.Web.Mvc;

namespace Project.Web.Controllers
{
    public class HomeController : Controller
    {
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

        public ActionResult Index()
        {
            ViewData["categories"] = categories;
            return View();
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