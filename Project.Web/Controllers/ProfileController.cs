using Project.BusinessLogic.Core;
using Project.BusinessLogic.DBModel;
using Project.Domain.Entities.Profile;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Project.Web.Controllers
{
    [JwtAuthorize]
    public class ProfileController : Controller
    {
        private readonly DatabaseContext _context = new DatabaseContext();

        public async Task<ActionResult> Index()
        {
            var claimsPrincipal = this.User as ClaimsPrincipal;

            if (claimsPrincipal != null)
            {
                var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;

                if (claimsIdentity != null && claimsIdentity.IsAuthenticated)
                {
                    var Id = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                    var Email = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
                    var Name = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
                    Debug.WriteLine("Id is " + Id);
                    var IdAsInt = int.Parse(Id);
                    
                    var user = await _context.Users.Include("Products.Categories")
                        .FirstOrDefaultAsync(u => u.Id == IdAsInt);

                    var viewModel = new ProfileViewModel
                    {
                        Id = Id,
                        Email = Email,
                        Name = Name,
                        Products = user.Products.Select(product => new Domain.Entities.Product.ProductViewModel
                                    {
                                        Id = product.Id,
                                        Title = product.Title,
                                        Price = product.Price,
                                        CreatedAt = product.CreatedAt,
                                        CategoryNames = product.Categories != null
                                                        ? product.Categories.Select(c => c.Name).ToList()
                                                        : new List<string>()
                                    })
                                    .OrderByDescending(p => p.CreatedAt)
                                    .ToList()
                    };

                    return View(viewModel);
                }
            }

            return View();
        }
    }
}