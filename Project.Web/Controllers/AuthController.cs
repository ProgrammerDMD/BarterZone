using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Project.BusinessLogic.Core;
using Project.BusinessLogic.DBModel;
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
                string token = await _userService.LoginUser(model.Email, model.Password);

                if (token != null)
                {
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
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool registered = await _userService.RegisterUser(model.Email, model.Password);

                if (registered)
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