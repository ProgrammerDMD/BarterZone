using Project.Web.Models;
using System.Web.Mvc;
using Project.BusinessLogic.Core;

namespace Project.Web.Controllers
{
    public class SignupController : Controller
    {
        private readonly UserService _userService = new UserService();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SignupViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View(model);
            }

            if (_userService.EmailExists(model.Email))
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                return View(model);
            }

            _userService.RegisterUser(model.Email, model.Password);

            return RedirectToAction("/auth");
        }
    }
}