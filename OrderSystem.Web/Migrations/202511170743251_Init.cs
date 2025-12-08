namespace OrderSystem.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        MenuId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Price = c.Int(nullable: false),
                        Category = c.String(nullable: false, maxLength: 50),
                        MenuTime = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.MenuId);
            
            CreateTable(
                "dbo.MenuOptionGroups",
                c => new
                    {
                        MenuId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MenuId, t.GroupId });
            
            CreateTable(
                "dbo.OptionGroups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false, maxLength: 100),
                        SelectMode = c.String(nullable: false, maxLength: 10),
                        IsRequired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        OptionId = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        OptionName = c.String(nullable: false, maxLength: 100),
                        ExtraPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OptionId);
            
            CreateTable(
                "dbo.OrderItemOptions",
                c => new
                    {
                        OrderItemId = c.Int(nullable: false),
                        OptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderItemId, t.OptionId });
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderItemId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        MenuId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        KitchenStatus = c.String(nullable: false, maxLength: 20),
                        StatusUpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrderItemId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        TableId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        OrderTime = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, maxLength: 20),
                        NumPeople = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        TableId = c.Int(nullable: false, identity: true),
                        TableName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.TableId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Role = c.String(nullable: false, maxLength: 50),
                        PasswordHash = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Tables");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
            DropTable("dbo.OrderItemOptions");
            DropTable("dbo.Options");
            DropTable("dbo.OptionGroups");
            DropTable("dbo.MenuOptionGroups");
            DropTable("dbo.MenuItems");
        }
    }
}
