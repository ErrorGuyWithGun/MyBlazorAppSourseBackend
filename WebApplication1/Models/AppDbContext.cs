using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using WebApplication1.Configuration;
using WebApplication1.Controllers;
using WebApplication1.Models.Authentication;
using WebApplication1.Models.Authentication.LogIn;
using WebApplication1.Models.Authentication.SignUp;
using WebApplication1.Models.Inventory;

namespace WebApplication1.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public AppDbContext()
        { }
        public DbSet<User> User { get; set; }
        public DbSet<RegisterUser> RegisterUser { get; set; }
        public DbSet<LoginModel> LoginModel { get; set; }

        public DbSet<InventoryModel> InventoryModel { get; set; }
        public DbSet<InventoryAccessModel> InventoryAccessModel { get; set; }

        public DbSet<CategoryModel> CategoryModel { get; set; }
        public DbSet<ItemModel> itemModel { get; set; }
        public DbSet<DiscussionModel> DiscussionModel { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RegisterUserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new DiscussiomConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new LoginModelConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryAccessConfiguration());
            SeedRoles(modelBuilder);
            SeedCategory(modelBuilder);
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Id = "admin-role", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin", },
                new IdentityRole() { Id = "user-role", Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
                );

        }
        //private void SeedTags(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<TagsModel>().HasData
        //        (
        //        new TagsModel() { Id="office-tag", Name="Office" },
        //        new TagsModel() { Id= "home-tag", Name="Home" }
        //        );

        //}

        private void SeedCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryModel>().HasData
               (
                new CategoryModel { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Book" },
                new CategoryModel { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Furniture" },
                new CategoryModel { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "Equipment" },
                new CategoryModel { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Name = "Other" }
               );
        }

    }
}
