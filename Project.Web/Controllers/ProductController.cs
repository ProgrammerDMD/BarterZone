using System.Diagnostics;
using System.Web.Mvc;

namespace Project.Web.Controllers
{
    public class ProductController : Controller
    {
        [Route("/Details/{id}")]
        public ActionResult Details(int id)
        {
            Debug.WriteLine("ProductController " + id);
            return View();
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
    }
}