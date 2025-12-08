namespace OrderSystem.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixModels : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.OrderItems", "OrderId");
            CreateIndex("dbo.OrderItems", "MenuId");
            AddForeignKey("dbo.OrderItems", "MenuId", "dbo.MenuItems", "MenuId", cascadeDelete: true);
            AddForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders", "OrderId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderItems", "MenuId", "dbo.MenuItems");
            DropIndex("dbo.OrderItems", new[] { "MenuId" });
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
        }
    }
}
