using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FurnitureStore.Migrations
{
    /// <inheritdoc />
    public partial class SeedFurnitureData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "Comfortable furniture for relaxing and entertaining.", "https://images.unsplash.com/photo-1555041469-a586c61ea9bc", "Living Room" },
                    { 2, "Create a calm and comfortable bedroom.", "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85", "Bedroom" },
                    { 3, "Tables and chairs designed for gathering.", "https://images.unsplash.com/photo-1617806118233-18e1de247200", "Dining" },
                    { 4, "Stylish storage solutions for every room.", "https://images.unsplash.com/photo-1595428774223-ef52624120d2", "Storage" },
                    { 5, "Create a productive and beautiful workspace.", "https://images.unsplash.com/photo-1497366811353-6870744d04b2", "Office" },
                    { 6, "Furniture for enjoying your outdoor space.", "https://images.unsplash.com/photo-1600210492486-724fe5c67fb0", "Outdoor" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "IsFeatured", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "A comfortable three-seater sofa with a modern minimalist design.", "https://images.unsplash.com/photo-1555041469-a586c61ea9bc", true, "Haven Three-Seater Sofa", 12999m },
                    { 2, 1, "A soft accent chair perfect for reading corners and living rooms.", "https://images.unsplash.com/photo-1567538096630-e0c55bd6374c", true, "Luna Accent Chair", 4999m },
                    { 3, 3, "Solid oak-inspired dining table with a clean contemporary finish.", "https://images.unsplash.com/photo-1617806118233-18e1de247200", true, "Oak Dining Table", 8999m },
                    { 4, 3, "Minimal dining chair designed for everyday comfort.", "https://images.unsplash.com/photo-1503602642458-232111445657", false, "Modern Dining Chair", 1899m },
                    { 5, 2, "A simple upholstered bed frame with a soft neutral finish.", "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85", true, "Haven Queen Bed", 10999m },
                    { 6, 2, "Compact bedside table with a drawer and open storage.", "https://images.unsplash.com/photo-1532372576444-dda954194ad0", false, "Oak Bedside Table", 2499m },
                    { 7, 4, "Elegant storage cabinet for books, crockery and accessories.", "https://images.unsplash.com/photo-1595428774223-ef52624120d2", true, "Nordic Storage Cabinet", 6999m },
                    { 8, 5, "Clean and functional desk for your home office.", "https://images.unsplash.com/photo-1497366811353-6870744d04b2", false, "Minimal Desk", 4499m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
