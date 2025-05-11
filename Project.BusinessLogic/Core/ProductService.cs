using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Project.BusinessLogic.DBModel;
using Project.Domain.Entities;

namespace Project.BusinessLogic.Core
{
    public class ProductService
    {
        private readonly DatabaseContext _db = new DatabaseContext();

        public async Task<ProductViewModel> GetProductsByPage(int page, int pageSize)
        {
            if (page < 1) page = 1;

            var skipAmount = (page - 1) * pageSize;
            var productsQuery = _db.Products.OrderBy(p => p.Id);

            var pagedProducts = await productsQuery
                .Skip(skipAmount)
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
                    Categories = p.Categories
                }).ToList(),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize)
            };
        }
    }
}