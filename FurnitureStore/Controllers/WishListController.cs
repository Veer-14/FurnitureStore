using FurnitureStore.Data;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Controllers
{
    public class WishlistController : Controller
    {
        private readonly FurnitureDbContext _context;

        public WishlistController(FurnitureDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // VIEW WISHLIST
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var wishlistItems = await _context.WishlistItems
                .Include(w => w.Product)
                .ThenInclude(p => p!.Category)
                .ToListAsync();

            var products = wishlistItems
                .Where(w => w.Product != null)
                .Select(w => w.Product!)
                .ToList();

            return View(products);
        }

        // =====================================================
        // ADD TO WISHLIST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId)
        {
            // Check that the product exists
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            // Check if it is already in the wishlist
            var alreadyExists = await _context.WishlistItems
                .AnyAsync(w => w.ProductId == productId);

            if (!alreadyExists)
            {
                var wishlistItem = new WishlistItem
                {
                    ProductId = productId
                };

                _context.WishlistItems.Add(wishlistItem);

                await _context.SaveChangesAsync();
            }

            // Take the user directly to their wishlist
            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // REMOVE FROM WISHLIST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            var wishlistItem = await _context.WishlistItems
                .FirstOrDefaultAsync(
                    w => w.ProductId == productId);

            if (wishlistItem != null)
            {
                _context.WishlistItems.Remove(wishlistItem);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}