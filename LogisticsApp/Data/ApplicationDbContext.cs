using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;
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
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
        public DbSet<FactoryProduct> FactoryProducts { get; set; }
        public DbSet<LoadedProduct> LoadedProducts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PortalUser>()
                .HasOne(pu => pu.Factory)
                .WithOne(f => f.PortalUser)
                .HasForeignKey<Factory>(f => f.PortalUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PortalUser>()
                .HasOne(pu => pu.Shop)
                .WithOne(s => s.PortalUser)
                .HasForeignKey<Shop>(s => s.PortalUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PortalUser>()
                .HasOne(pu => pu.Truck)
                .WithOne(t => t.PortalUser)
                .HasForeignKey<Truck>(t => t.PortalUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShopProduct>()
                .HasOne(sp => sp.Shop)
                .WithMany(s => s.ShopProducts)
                .HasForeignKey(sp => sp.ShopId);
            modelBuilder.Entity<ShopProduct>()
                .HasOne(sp => sp.Product)
                .WithMany(p => p.ShopProducts)
                .HasForeignKey(sp => sp.ProductId);

            modelBuilder.Entity<OrderedProduct>()
                .HasOne(op => op.Shop)
                .WithMany(s => s.OrderedProducts)
                .HasForeignKey(op => op.ShopId);
            modelBuilder.Entity<OrderedProduct>()
                .HasOne(op => op.FactoryProduct)
                .WithMany(fp => fp.OrderedProducts)
                .HasForeignKey(op => op.FactoryProductId);

            modelBuilder.Entity<FactoryProduct>()
                .HasOne(fp => fp.Factory)
                .WithMany(f => f.FactoryProducts)
                .HasForeignKey(fp => fp.FactoryId);
            modelBuilder.Entity<FactoryProduct>()
                .HasOne(fp => fp.Product)
                .WithMany(p => p.FactoryProducts)
                .HasForeignKey(fp => fp.ProductId);

            modelBuilder.Entity<LoadedProduct>()
                .HasOne(lp => lp.Truck)
                .WithMany(t => t.LoadedProducts)
                .HasForeignKey(lp => lp.TruckId);
            modelBuilder.Entity<LoadedProduct>()
                .HasOne(lp => lp.OrderedProduct)
                .WithMany(op => op.LoadedProducts)
                .HasForeignKey(lp => lp.OrderedProductId);
        }
    }
}
