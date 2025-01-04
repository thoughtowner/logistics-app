using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<PortalUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Factory> Factories { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<ShopProduct> ShopProducts { get; set; }
        public DbSet<FactoryProduct> FactoryProducts { get; set; }
        public DbSet<TruckProduct> TruckProducts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShopProduct>()
                .HasOne(sp => sp.Shop)
                .WithMany(s => s.ShopProducts)
                .HasForeignKey(sp => sp.ShopId);
            modelBuilder.Entity<ShopProduct>()
                .HasOne(sp => sp.Product)
                .WithMany(p => p.ShopProducts)
                .HasForeignKey(sp => sp.ProductId);

            modelBuilder.Entity<FactoryProduct>()
                .HasOne(fp => fp.Factory)
                .WithMany(f => f.FactoryProducts)
                .HasForeignKey(fp => fp.FactoryId);
            modelBuilder.Entity<FactoryProduct>()
                .HasOne(fp => fp.Product)
                .WithMany(p => p.FactoryProducts)
                .HasForeignKey(fp => fp.ProductId);

            modelBuilder.Entity<TruckProduct>()
                .HasOne(fp => fp.Truck)
                .WithMany(f => f.TruckProducts)
                .HasForeignKey(fp => fp.TruckId);
            modelBuilder.Entity<TruckProduct>()
                .HasOne(fp => fp.Product)
                .WithMany(p => p.TruckProducts)
                .HasForeignKey(fp => fp.ProductId);
        }
    }
}
