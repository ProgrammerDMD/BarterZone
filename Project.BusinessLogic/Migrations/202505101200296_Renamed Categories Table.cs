namespace Project.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedCategoriesTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CategoryTables", newName: "Categories");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Categories", newName: "CategoryTables");
        }
    }
}
