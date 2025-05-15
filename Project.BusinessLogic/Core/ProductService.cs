using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Project.BusinessLogic.DBModel;
using Project.Domain.Entities;

namespace Project.BusinessLogic.Core
{
    public class ProductService
    {
        private readonly DatabaseContext _db = new DatabaseContext();

        public static readonly Dictionary<int, string> categories = new Dictionary<int, string>
        {
            [1] = "Electronics",
            [2] = "Clothing & Fashion",
            [3] = "Collectibles",
            [4] = "Books & Media",
            [5] = "Home & Furniture",
            [6] = "Toys & Games",
            [7] = "Sports & Outdoors",
            [8] = "Automotive",
            [9] = "Beauty & Health",
            [10] = "Tools & DIY",
            [11] = "Musical Instruments",
            [12] = "Pet Supplies",
            [13] = "Office & School Supplies",
            [14] = "Tickets & Experiences"
        };
        
        public async Task<int> CreateProduct(ProductCreateViewModel model, int creatorId)
        {
            if (model == null) return -1;
            
            string imagePath = null;
            if (model.Image != null && model.Image.ContentLength > 0)
            {
                var fileName = Path.GetFileNameWithoutExtension(model.Image.FileName);
                var extension = Path.GetExtension(model.Image.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                var directoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Uploads");

                if (directoryPath != null && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                    imagePath = Path.Combine(directoryPath, fileName);
                    model.Image.SaveAs(imagePath);
                }
            }
            
            var product = new Product
            {
                Title = model.Title,
                Image = imagePath,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.Category,
                CreatorId = creatorId,
                CreatedAt = DateTime.UtcNow
            };
            
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product.Id;
        }
        
        public async Task<ProductViewModel> GetProductsByPage(int page, int pageSize, string search = null)
        {
            if (page < 0) page = 0;

            IQueryable<Product> productsQuery = _db.Products;
            var offset = page * pageSize;

            if (!string.IsNullOrEmpty(search))
            {
                var searchTermLower = search.ToLower();
                productsQuery = productsQuery.Where(p =>
                    (p.Title != null && p.Title.ToLower().Contains(searchTermLower)) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchTermLower))
                );
            }
            
            var orderedQuery = productsQuery.OrderBy(p => p.Id);
            
            var pagedProducts = await orderedQuery
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();

            var totalProducts = await productsQuery.CountAsync();
            
            return new ProductViewModel
            {
                Products = pagedProducts.Select(p => new Product
                {
                    Id = p.Id,
                    CreatorId = p.CreatorId,
                    Title = p.Title,
                    Price = p.Price,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    CategoryId = p.CategoryId
                }).ToList(),
                Search = search,
                CurrentPage = page + 1,
                TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize),
                PageSize = pageSize
            };
        }
    }
}