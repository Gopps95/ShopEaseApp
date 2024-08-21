using Microsoft.EntityFrameworkCore;


namespace ShopEaseApp.Models
{
    public class ShoppingDataContext
    {

        public class ShoppingModelDB : DbContext
        {
            public ShoppingModelDB(DbContextOptions<ShoppingModelDB> options) : base(options)
            {

            }
            public DbSet<Category> Categories { get; set; }
            public DbSet<User> User { get; set; }

            public DbSet<Product> Products { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderDetail> OrderDetails { get; set; }
            public DbSet<Payment> Payments { get; set; }

          //  public DbSet<RegistrationModel> Register { get; set; }
            //        protected override void OnModelCreating(ModelBuilder modelBuilder)
            //        {
            //            modelBuilder
            //.Entity<OrderDetail>(builder =>
            //{
            //    builder.HasNoKey();
            //    builder.ToTable("OrderDetails");
            //});
            //        }

        }
    }
}
