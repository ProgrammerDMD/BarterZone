using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project.BusinessLogic.Core;
using Project.BusinessLogic.Core.Filters;
using Project.BusinessLogic.Core.JWT;
using Project.Domain.Entities;
using Project.Domain.Entities.Authentication;

namespace Project.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserService _userService = new UserService();

        public ActionResult Index()
        {
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        [RedirectIfAuthorized("Home")]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpGet]
        [RedirectIfAuthorized("Home")]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userService.LoginUser(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            var cookie = CreateJWTCookie(user);
            if (cookie == null) return View(model);

            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userService.RegisterUser(model.Email, model.Name, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Email already exists!");
                return View(model);
            }

            var cookie = CreateJWTCookie(user);
            if (cookie == null) return View(model);

            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            JwtAuthorizer.DeleteAuthCookie(HttpContext);
            return RedirectToAction("Index", "Home");
        }

        private HttpCookie CreateJWTCookie(User user)
        {
            var secretKey = Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["Jwt:Key"]);
            var issuer = ConfigurationManager.AppSettings["Jwt:Issuer"];
            var expiryMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["Jwt:ExpiryMinutes"]);

            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer))
            {
                ModelState.AddModelError("", "JWT configuration is missing or invalid.");
                return null;
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToLowerInvariant())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = issuer,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var generatedToken = tokenHandler.WriteToken(securityToken);

            var tokenCookie = new HttpCookie("AuthToken", generatedToken)
            {
                HttpOnly = true,
                Secure = Request.IsSecureConnection,
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Add(tokenCookie);
            return tokenCookie;
        }
    }
}