using Project.Domain;
using System.Data.Entity;

namespace Project.BusinessLogic.Core
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection") { }

        public DbSet<User> Users { get; set; }
    }
}