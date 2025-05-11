using System.Data.Entity.Migrations;
using Project.BusinessLogic.DBModel;
using Project.Domain.Entities;

namespace Project.BusinessLogic.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DatabaseContext context)
        {
            context.Categories.AddOrUpdate(
                c => c.Name,
                new Category { Name = "Electronics" },
                new Category { Name = "Clothing & Fashion" },
                new Category { Name = "Collectibles" },
                new Category { Name = "Books & Media" },
                new Category { Name = "Home & Furniture" },
                new Category { Name = "Toys & Games" },
                new Category { Name = "Sports & Outdoors" },
                new Category { Name = "Automotive" },
                new Category { Name = "Beauty & Health" },
                new Category { Name = "Tools & DIY" },
                new Category { Name = "Musical Instruments" },
                new Category { Name = "Pet Supplies" },
                new Category { Name = "Office & School Supplies" },
                new Category { Name = "Tickets & Experiences" }
            );
        }
    }
}