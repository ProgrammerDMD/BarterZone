using Project.BusinessLogic.Core;
using Project.Domain.Entities.Profile;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

            if (claimsPrincipal != null)
            {
                var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;

                if (claimsIdentity != null && claimsIdentity.IsAuthenticated)
                {
                    var viewModel = new ProfileViewModel
                    {
                        Id = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
                        Email = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Email)?.Value,
                        Name = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Name)?.Value
                    };
                    return View(viewModel);
                }
            }

            return View();
        }
    }
}