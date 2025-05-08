using Microsoft.IdentityModel.Tokens;
using Project.BusinessLogic.Core;
using Project.Domain.Entities.Authentication;
using Project.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userService.LoginUser(model.Email, model.Password);

                if (user != null)
                {
                    var secretKey = Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["Jwt:Key"]);
                    var issuer = ConfigurationManager.AppSettings["Jwt:Issuer"];
                    var expiryMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["Jwt:ExpiryMinutes"]);

                    if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer))
                    {
                        ModelState.AddModelError("", "JWT configuration is missing or invalid.");
                        return View(model);
                    }

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, user.Role)
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
                    string generatedToken = tokenHandler.WriteToken(securityToken);

                    HttpCookie tokenCookie = new HttpCookie("AuthToken", generatedToken)
                    {
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection,
                        Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                        SameSite = SameSiteMode.Strict
                    };

                    Response.Cookies.Add(tokenCookie);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userService.RegisterUser(model.Email, model.Password);

                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email already exists!"); // Further error TODO
                }
            }
            return View(model);
        }
    }
}