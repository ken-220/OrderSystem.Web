namespace OrderSystem.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInventoryTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        IngredientId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Unit = c.String(nullable: false, maxLength: 20),
                        CurrentStock = c.Int(nullable: false),
                        MinimumStock = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IngredientId);
            
            CreateTable(
                "dbo.MenuIngredients",
                c => new
                    {
                        MenuId = c.Int(nullable: false),
                        IngredientId = c.Int(nullable: false),
                        QuantityUsed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MenuId, t.IngredientId })
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .ForeignKey("dbo.MenuItems", t => t.MenuId, cascadeDelete: true)
                .Index(t => t.MenuId)
                .Index(t => t.IngredientId);
            
            CreateTable(
                "dbo.PurchaseHistories",
                c => new
                    {
                        PurchaseHistoryId = c.Int(nullable: false, identity: true),
                        IngredientId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                        Note = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.PurchaseHistoryId)
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .Index(t => t.IngredientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseHistories", "IngredientId", "dbo.Ingredients");
            DropForeignKey("dbo.MenuIngredients", "MenuId", "dbo.MenuItems");
            DropForeignKey("dbo.MenuIngredients", "IngredientId", "dbo.Ingredients");
            DropIndex("dbo.PurchaseHistories", new[] { "IngredientId" });
            DropIndex("dbo.MenuIngredients", new[] { "IngredientId" });
            DropIndex("dbo.MenuIngredients", new[] { "MenuId" });
            DropTable("dbo.PurchaseHistories");
            DropTable("dbo.MenuIngredients");
            DropTable("dbo.Ingredients");
        }
    }
}
