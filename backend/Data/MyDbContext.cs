using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;


namespace backend.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OS> OS { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet <Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ShipmentMethod> ShipmentMethods { get; set; }
        public DbSet<ShippingDetails> ShippingDetails { get; set; }
        public DbSet<Specs> Specs { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Warranty> Warranties { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthToken>()
               .Property(t => t.TimeGenerated)
               .HasDefaultValueSql("GETUTCDATE()");

            
            modelBuilder.Entity<AuthToken>()
                .HasIndex(t => t.Value)
                .IsUnique();

           modelBuilder.Entity<Brand>().HasData(
            new Brand
            {
                BrandId = 1,
                Name = "Apple"
            },
            new Brand
            {
                BrandId = 2,
                Name = "Samsung"
            }
            );

            modelBuilder.Entity<Category>().HasData(
            new Category
            {
                CategoryId = 1,
                Name = "Smartphones"
            }
            );

            modelBuilder.Entity<Supplier>().HasData(
            new Supplier
            {
                SupplierId = 1,
                SupplierName = "Main Supplier",
                PhoneNumber = "+38761111222",
                Address = "Sarajevo, BiH"
            }
            );


            modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = 1,
                ProductName = "iPhone 14",
                Model = "A2882",
                Price = 999,
                BrandId = 1,       
                SupplierId = 1,
                CategoryId = 1,   
                PromotionId = null
            },
            new Product
            {
                ProductId = 2,
                ProductName = "iPhone 13",
                Model = "A2633",
                Price = 799,
                BrandId = 1,
                SupplierId = 1,
                CategoryId = 1,
                PromotionId = null
            },
            new Product
            {
                ProductId = 3,
                ProductName = "Samsung Galaxy S23",
                Model = "SM-S911B",
                Price = 899,
                BrandId = 2,       
                SupplierId = 1,
                CategoryId = 1,
                PromotionId = null
            },
            new Product
            {
                ProductId = 4,
                ProductName = "Samsung Galaxy A54",
                Model = "SM-A546B",
                Price = 449,
                BrandId = 2,
                SupplierId = 1,
                CategoryId = 1,
                PromotionId = null
            }
            );








        }
    }
}
