using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Project.BusinessLogic.DBModel;
using Project.Domain.Entities;
using Project.Domain.Entities.Login;

namespace Project.Web.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
                public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool credentialsAreValid = false;
                
                using (var dbContext = new UserContext())
                {
                    UserDbTable user = await dbContext.Users
                        .FirstOrDefaultAsync(u => u.Email == model.Email);
                    
                    if (user != null)
                    {
                        bool isPasswordCorrect = (user.Password == model.Password);

                        if (isPasswordCorrect)
                        {
                            credentialsAreValid = true;
                        }
                    }
                }
                
                if (credentialsAreValid)
                {
                    return RedirectToAction("Index", "Home"); // Redirect to Home/Index
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }
    }
}