using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Project.BusinessLogic.Core;
using Project.Domain.Entities;

namespace Project.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService = new ProductService();
        
        [Route("/Details/{id}")]
        public ActionResult Details(int id)
        {
            return View();
        }
        
        [JwtAuthorize]
        [HttpGet]
        public ActionResult Create()
        {
            ViewData["categories"] = ProductService.categories;
            return View();
        }

        [JwtAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "The submitted form is invalid!");
                return View(model);
            }
            
            var claimsIdentity = User?.Identity as ClaimsIdentity;
            if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            var id = int.Parse(claimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
            var result = await _productService.CreateProduct(model, id);
            if (result == -1)
            {
                ModelState.AddModelError("", "The submitted form is invalid!");
                return View(model);
            }
            
            return RedirectToAction("Details", "Product", new { id = id });
        }
    }
}