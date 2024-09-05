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
         
            public DbSet<User> User { get; set; }

            public DbSet<Product> Products { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderDetail> OrderDetails { get; set; }
         

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<OrderDetail>()
            .HasKey(od => new { od.OrderID, od.ProductID });

                modelBuilder.Entity<Order>()
          .HasMany(o => o.OrderDetails)
          .WithOne(od => od.Order)
          .HasForeignKey(od => od.OrderID);


                modelBuilder.Entity<OrderDetail>()
           .Property(od => od.UnitPrice)
           .HasColumnType("decimal(18, 2)");
                // base.OnModelCreating(modelBuilder);
            }


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
