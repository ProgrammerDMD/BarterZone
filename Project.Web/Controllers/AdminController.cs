using System.Web.Mvc;
using Project.BusinessLogic.Core;

namespace Project.Web.Controllers
{
    [JwtAuthorize("Admin")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}