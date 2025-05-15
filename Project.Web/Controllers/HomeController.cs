using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Project.BusinessLogic.Core;

namespace Project.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductService _productService = new ProductService();

        public async Task<ActionResult> Index(string search, int? page)
        {
            ViewData["categories"] = ProductService.categories;
            int currentPage = page ?? 1;
            
            var products = await _productService.GetProductsByPage(currentPage - 1, 25, search);
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