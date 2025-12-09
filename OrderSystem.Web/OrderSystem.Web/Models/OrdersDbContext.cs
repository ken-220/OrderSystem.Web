using System.Data.Entity;

namespace OrderSystem.Web.Models
{
    public class OrdersDbContext : DbContext
    {
        // Web.config の <connectionStrings> で定義した "OrdersDb" を利用
        public OrdersDbContext() : base("name=OrdersDb")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<OptionGroup> OptionGroups { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<MenuOptionGroup> MenuOptionGroups { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemOption> OrderItemOptions { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            mb.Entity<MenuOptionGroup>()
              .HasKey(x => new { x.MenuId, x.GroupId });

            mb.Entity<OrderItemOption>()
              .HasKey(x => new { x.OrderItemId, x.OptionId });

            mb.Entity<OrderItem>()
              .Property(x => x.Quantity)
              .IsRequired();

            base.OnModelCreating(mb);
        }
    }
}
