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
                    Categories = p.Categories
                }).ToList(),
                Search = search,
                CurrentPage = page + 1,
                TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize),
                PageSize = pageSize
            };
        }
    }
}