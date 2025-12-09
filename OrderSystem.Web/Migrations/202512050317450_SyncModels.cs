namespace OrderSystem.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SyncModels : DbMigration
    {
        public override void Up()
        {
           
            AddColumn("dbo.Orders", "PaidAt", c => c.DateTime());
            AddColumn("dbo.Orders", "PaymentMethod", c => c.String());
            AddColumn("dbo.Orders", "PaidAmount", c => c.Int());
           
            AlterColumn("dbo.OrderItems", "KitchenStatus", c => c.String());
            AlterColumn("dbo.Orders", "NumPeople", c => c.Int());
            CreateIndex("dbo.OrderItemOptions", "OrderItemId");
            CreateIndex("dbo.OrderItemOptions", "OptionId");
            CreateIndex("dbo.Orders", "TableId");
            AddForeignKey("dbo.OrderItemOptions", "OptionId", "dbo.Options", "OptionId", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "TableId", "dbo.Tables", "TableId", cascadeDelete: true);
            AddForeignKey("dbo.OrderItemOptions", "OrderItemId", "dbo.OrderItems", "OrderItemId", cascadeDelete: true);
            
        }
        
        public override void Down()
        {
           
           
            DropForeignKey("dbo.Orders", "TableId", "dbo.Tables");
            DropForeignKey("dbo.OrderItemOptions", "OptionId", "dbo.Options");
            DropIndex("dbo.Orders", new[] { "TableId" });
            DropIndex("dbo.OrderItemOptions", new[] { "OptionId" });
            DropIndex("dbo.OrderItemOptions", new[] { "OrderItemId" });
            AlterColumn("dbo.Orders", "NumPeople", c => c.Int(nullable: false));
            AlterColumn("dbo.OrderItems", "KitchenStatus", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.Users", "Password");
            DropColumn("dbo.Orders", "PaidAmount");
            DropColumn("dbo.Orders", "PaymentMethod");
            DropColumn("dbo.Orders", "PaidAt");
            DropColumn("dbo.OrderItemOptions", "OrderItemOptionId");
        }
    }
}
