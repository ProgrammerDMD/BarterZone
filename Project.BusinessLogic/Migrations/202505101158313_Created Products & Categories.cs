namespace Project.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedProductsCategories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatorId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Price = c.Double(nullable: false),
                        Description = c.String(nullable: false, maxLength: 1024),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.ProductTableCategoryTables",
                c => new
                    {
                        ProductTable_Id = c.Int(nullable: false),
                        CategoryTable_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductTable_Id, t.CategoryTable_Id })
                .ForeignKey("dbo.Products", t => t.ProductTable_Id, cascadeDelete: true)
                .ForeignKey("dbo.CategoryTables", t => t.CategoryTable_Id, cascadeDelete: true)
                .Index(t => t.ProductTable_Id)
                .Index(t => t.CategoryTable_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "CreatorId", "dbo.Users");
            DropForeignKey("dbo.ProductTableCategoryTables", "CategoryTable_Id", "dbo.CategoryTables");
            DropForeignKey("dbo.ProductTableCategoryTables", "ProductTable_Id", "dbo.Products");
            DropIndex("dbo.ProductTableCategoryTables", new[] { "CategoryTable_Id" });
            DropIndex("dbo.ProductTableCategoryTables", new[] { "ProductTable_Id" });
            DropIndex("dbo.Products", new[] { "CreatorId" });
            DropTable("dbo.ProductTableCategoryTables");
            DropTable("dbo.Products");
            DropTable("dbo.CategoryTables");
        }
    }
}
