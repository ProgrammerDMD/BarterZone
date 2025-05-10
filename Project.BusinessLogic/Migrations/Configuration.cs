namespace Project.BusinessLogic.Migrations
{
    using Project.Domain.Entities.Database;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Project.BusinessLogic.DBModel.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Project.BusinessLogic.DBModel.DatabaseContext context)
        {
            context.Categories.AddOrUpdate(
                c => c.Name,
                new CategoryTable { Name = "Electronics" },
                new CategoryTable { Name = "Clothing & Fashion" },
                new CategoryTable { Name = "Collectibles" },
                new CategoryTable { Name = "Books & Media" },
                new CategoryTable { Name = "Home & Furniture" },
                new CategoryTable { Name = "Toys & Games" },
                new CategoryTable { Name = "Sports & Outdoors" },
                new CategoryTable { Name = "Automotive" },
                new CategoryTable { Name = "Beauty & Health" },
                new CategoryTable { Name = "Tools & DIY" },
                new CategoryTable { Name = "Musical Instruments" },
                new CategoryTable { Name = "Pet Supplies" },
                new CategoryTable { Name = "Office & School Supplies" },
                new CategoryTable { Name = "Tickets & Experiences" }
            );
        }
    }
}
