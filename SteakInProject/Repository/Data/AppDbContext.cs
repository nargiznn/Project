using System;
using System.Reflection;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Award> Awards { get; set; }
        public DbSet<Chef> Chefs { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<ChefImage> ChefImages { get; set; }
        public DbSet<ChefPosition> ChefPositions { get; set; }
        public DbSet<SocialMediaLink> SocialMediaLinks { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Cuisine> Cuisines { get; set; }
        public DbSet<Client> Clients { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentReply> CommentReplies { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<GalleryCategory> GalleryCategories { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<MealPackage> MealPackages { get; set; }
        public DbSet<LunchSet> LunchSets { get; set; }


        public DbSet<MealPackageProduct> MealPackageProducts { get; set; }
        public DbSet<LunchSetProduct> LunchSetProducts { get; set; }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<WelcomeInfo> WelcomeInfos { get; set; }
        public DbSet<WelcomeImage> WelcomeImages { get; set; }
        public DbSet<Event> Events { get; set; }

        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<SpecialCategory> SpecialCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<RestaurantTable> RestaurantTables { get; set; }


        public DbSet<AwardLogo> AwardLogos { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Banner> Banners { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Comment>()
            //            .HasMany(c => c.Replies)
            //            .WithOne(r => r.Comment)
            //            .HasForeignKey(r => r.CommentId);


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

            modelBuilder.Entity<MealPackageProduct>()
       .HasKey(mpp => new { mpp.MealPackageId, mpp.ProductId });



            //modelBuilder.Entity<MealPackageProduct>()
            //    .HasOne(mpp => mpp.MealPackage)
            //    .WithMany(mp => mp.Products)
            //    .HasForeignKey(mpp => mpp.MealPackageId);

            modelBuilder.Entity<LunchSetProduct>()
                .HasKey(lsp => new { lsp.LunchSetId, lsp.ProductId });

            modelBuilder.Entity<LunchSetProduct>()
                .HasOne(lsp => lsp.LunchSet)
                .WithMany(ls => ls.LunchSetProducts)
                .HasForeignKey(lsp => lsp.LunchSetId);

            modelBuilder.Entity<LunchSetProduct>()
                .HasOne(lsp => lsp.Product)
                .WithMany(p => p.LunchSetProducts)
                .HasForeignKey(lsp => lsp.ProductId);

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

