using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Project.BusinessLogic.Core;

namespace Project.Web.Controllers
{
    [JwtAuthorize]
    public class ProfileController : Controller
    {
        private readonly UserService _userService = new UserService();

        public async Task<ActionResult> Index()
        {
            var claimsIdentity = User?.Identity as ClaimsIdentity;
            if (claimsIdentity == null || !claimsIdentity.IsAuthenticated) return View();

            var id = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (id == null || !int.TryParse(id, out var userId)) return View();

            var profile = await _userService.GetProfileById(userId);
            return View(profile);
        }
    }
}