using FurnitureStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly FurnitureDbContext _context;

        public HomeController(FurnitureDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get categories from PostgreSQL
            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            // Get featured products from PostgreSQL
            var featuredProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsFeatured)
                .ToListAsync();

            return View(featuredProducts);
        }
    }
}