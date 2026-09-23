using FurnitureStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly FurnitureDbContext _context;

        public ProductsController(FurnitureDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // PRODUCT LIST
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            // Get all categories for the category navigation/filter
            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            // Start with all products
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            // Filter by category if one was selected
            if (categoryId.HasValue)
            {
                query = query.Where(
                    p => p.CategoryId == categoryId.Value);

                var category = await _context.Categories
                    .FirstOrDefaultAsync(
                        c => c.Id == categoryId.Value);

                ViewBag.CurrentCategory = category?.Name;
            }

            var products = await query.ToListAsync();

            return View(products);
        }

        // =====================================================
        // PRODUCT DETAILS
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}