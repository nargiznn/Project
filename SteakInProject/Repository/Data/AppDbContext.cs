using System;
using System.Reflection;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.Data
{
	public class AppDbContext:IdentityDbContext<AppUser>
	{
        public DbSet<Chef> Chefs { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<ChefImage> ChefImages { get; set; }
        public DbSet<ChefPosition> ChefPositions { get; set; }
        public DbSet<SocialMediaLink> SocialMediaLinks { get; set; }

        public DbSet<Cuisine> Cuisines { get; set; }


        public DbSet<Slider> Sliders { get; set; }
        public DbSet<WelcomeInfo> WelcomeInfos { get; set; }
        public DbSet<WelcomeImage> WelcomeImages { get; set; }
        public DbSet<Event> Events { get; set; }

        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<SpecialCategory> SpecialCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Award> Awards { get; set; }
        public DbSet<AwardLogo> AwardLogos { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
          .HasOne(p => p.Cuisine)
          .WithMany(c => c.Products)
          .HasForeignKey(p => p.CuisineId)
          .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chef>()
                .HasMany(c => c.ChefPosition)
                .WithOne(cp => cp.Chef)
                .HasForeignKey(cp => cp.ChefId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Chef>()
                .HasMany(c => c.ChefImages)
                .WithOne(ci => ci.Chef)
                .HasForeignKey(ci => ci.ChefId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

