using Project.BusinessLogic.Core;
using System.Diagnostics;
using System.Security.Claims;
using System.Web.Mvc;

namespace Project.Web.Controllers
{
    [JwtAuthorize]
    public class ProfileController : Controller
    {
        public ActionResult Index()
        {
            var claimsPrincipal = this.User as ClaimsPrincipal;
            return View();
        }
    }
}