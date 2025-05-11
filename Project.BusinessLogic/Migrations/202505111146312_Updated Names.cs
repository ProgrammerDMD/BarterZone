namespace Project.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedNames : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProductTableCategoryTables", newName: "ProductCategories");
            RenameColumn(table: "dbo.ProductCategories", name: "ProductTable_Id", newName: "Product_Id");
            RenameColumn(table: "dbo.ProductCategories", name: "CategoryTable_Id", newName: "Category_Id");
            RenameIndex(table: "dbo.ProductCategories", name: "IX_ProductTable_Id", newName: "IX_Product_Id");
            RenameIndex(table: "dbo.ProductCategories", name: "IX_CategoryTable_Id", newName: "IX_Category_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ProductCategories", name: "IX_Category_Id", newName: "IX_CategoryTable_Id");
            RenameIndex(table: "dbo.ProductCategories", name: "IX_Product_Id", newName: "IX_ProductTable_Id");
            RenameColumn(table: "dbo.ProductCategories", name: "Category_Id", newName: "CategoryTable_Id");
            RenameColumn(table: "dbo.ProductCategories", name: "Product_Id", newName: "ProductTable_Id");
            RenameTable(name: "dbo.ProductCategories", newName: "ProductTableCategoryTables");
        }
    }
}
