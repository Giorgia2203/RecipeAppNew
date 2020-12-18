using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeModel.Models;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.AspNetCore.Identity;

namespace RecipeModel.Data
{
    public class RecipeContext : IdentityDbContext<AppUser>
    {
        public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
        {

        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandIngredient> BrandIngredients { get; set; }
        public DbSet<FavouriteRecipe> FavouriteRecipes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>().ToTable("Brand");
            modelBuilder.Entity<BrandIngredient>().ToTable("BrandIngredient");
            modelBuilder.Entity<FavouriteRecipe>().ToTable("FavouriteRecipe");
            modelBuilder.Entity<Image>().ToTable("Image");
            modelBuilder.Entity<Ingredient>().ToTable("Ingredient");
            modelBuilder.Entity<Recipe>().ToTable("Recipe");
            modelBuilder.Entity<RecipeIngredient>().ToTable("RecipeIngredient");
            modelBuilder.Entity<Review>().ToTable("Review");
            modelBuilder.Entity<AppUser>().ToTable("User");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BrandIngredient>().HasKey(table => new { table.BrandId, table.IngredientId });
            modelBuilder.Entity<FavouriteRecipe>().HasKey(table => new { table.Id, table.RecipeId, table.AppUserId });
            modelBuilder.Entity<Image>().HasKey(table => new { table.Id, table.RecipeId, table.AppUserId });
            modelBuilder.Entity<RecipeIngredient>().HasKey(table => new { table.RecipeId, table.IngredientId });
            modelBuilder.Entity<Review>().HasKey(table => new { table.Id, table.AppUserId, table.RecipeId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=RecipeDB;Trusted_Connection=True;MultipleActiveResultSets=true",
                                        b => b.MigrationsAssembly("RecipeMVC"));
        }
    }
}
