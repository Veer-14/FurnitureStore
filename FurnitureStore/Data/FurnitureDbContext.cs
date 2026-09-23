using FurnitureStore.Models;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Data
{
    public class FurnitureDbContext : DbContext
    {
        public FurnitureDbContext(
            DbContextOptions<FurnitureDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product → Category relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Price precision
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // --------------------------------
            // CATEGORIES
            // --------------------------------

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Living Room",
                    Description = "Comfortable furniture for relaxing and entertaining.",
                    ImageUrl = "https://images.unsplash.com/photo-1555041469-a586c61ea9bc"
                },

                new Category
                {
                    Id = 2,
                    Name = "Bedroom",
                    Description = "Create a calm and comfortable bedroom.",
                    ImageUrl = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85"
                },

                new Category
                {
                    Id = 3,
                    Name = "Dining",
                    Description = "Tables and chairs designed for gathering.",
                    ImageUrl = "https://images.unsplash.com/photo-1617806118233-18e1de247200"
                },

                new Category
                {
                    Id = 4,
                    Name = "Storage",
                    Description = "Stylish storage solutions for every room.",
                    ImageUrl = "https://images.unsplash.com/photo-1595428774223-ef52624120d2"
                },

                new Category
                {
                    Id = 5,
                    Name = "Office",
                    Description = "Create a productive and beautiful workspace.",
                    ImageUrl = "https://images.unsplash.com/photo-1497366811353-6870744d04b2"
                },

                new Category
                {
                    Id = 6,
                    Name = "Outdoor",
                    Description = "Furniture for enjoying your outdoor space.",
                    ImageUrl = "https://images.unsplash.com/photo-1600210492486-724fe5c67fb0"
                }
            );

            // --------------------------------
            // PRODUCTS
            // --------------------------------

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Haven Three-Seater Sofa",
                    Description = "A comfortable three-seater sofa with a modern minimalist design.",
                    Price = 12999,
                    CategoryId = 1,
                    IsFeatured = true,
                    ImageUrl = "https://images.unsplash.com/photo-1555041469-a586c61ea9bc"
                },

                new Product
                {
                    Id = 2,
                    Name = "Luna Accent Chair",
                    Description = "A soft accent chair perfect for reading corners and living rooms.",
                    Price = 4999,
                    CategoryId = 1,
                    IsFeatured = true,
                    ImageUrl = "https://images.unsplash.com/photo-1567538096630-e0c55bd6374c"
                },

                new Product
                {
                    Id = 3,
                    Name = "Oak Dining Table",
                    Description = "Solid oak-inspired dining table with a clean contemporary finish.",
                    Price = 8999,
                    CategoryId = 3,
                    IsFeatured = true,
                    ImageUrl = "https://images.unsplash.com/photo-1617806118233-18e1de247200"
                },

                new Product
                {
                    Id = 4,
                    Name = "Modern Dining Chair",
                    Description = "Minimal dining chair designed for everyday comfort.",
                    Price = 1899,
                    CategoryId = 3,
                    IsFeatured = false,
                    ImageUrl = "https://images.unsplash.com/photo-1503602642458-232111445657"
                },

                new Product
                {
                    Id = 5,
                    Name = "Haven Queen Bed",
                    Description = "A simple upholstered bed frame with a soft neutral finish.",
                    Price = 10999,
                    CategoryId = 2,
                    IsFeatured = true,
                    ImageUrl = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85"
                },

                new Product
                {
                    Id = 6,
                    Name = "Oak Bedside Table",
                    Description = "Compact bedside table with a drawer and open storage.",
                    Price = 2499,
                    CategoryId = 2,
                    IsFeatured = false,
                    ImageUrl = "https://images.unsplash.com/photo-1532372576444-dda954194ad0"
                },

                new Product
                {
                    Id = 7,
                    Name = "Nordic Storage Cabinet",
                    Description = "Elegant storage cabinet for books, crockery and accessories.",
                    Price = 6999,
                    CategoryId = 4,
                    IsFeatured = true,
                    ImageUrl = "https://images.unsplash.com/photo-1595428774223-ef52624120d2"
                },

                new Product
                {
                    Id = 8,
                    Name = "Minimal Desk",
                    Description = "Clean and functional desk for your home office.",
                    Price = 4499,
                    CategoryId = 5,
                    IsFeatured = false,
                    ImageUrl = "https://images.unsplash.com/photo-1497366811353-6870744d04b2"
                }
            );
        }
    }
}