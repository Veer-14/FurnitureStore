using FurnitureStore.Models;
using FurnitureStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.Controllers
{
    public class WishlistController : Controller
    {
        private readonly FurnitureShop _store;

        public WishlistController(FurnitureShop store)
        {
            _store = store;
        }

        public IActionResult Index()
        {
            var products = _store.Wishlist
                .Select(w => _store.Products
                    .FirstOrDefault(p => p.Id == w.ProductId))
                .Where(p => p != null)
                .ToList();

            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int productId)
        {
            var product = _store.Products
                .FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            bool alreadyExists = _store.Wishlist
                .Any(w => w.ProductId == productId);

            if (!alreadyExists)
            {
                _store.Wishlist.Add(new WishlistItem
                {
                    Id = _store.Wishlist.Count + 1,
                    ProductId = productId
                });
            }

            // Take the user directly to their wishlist
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int productId)
        {
            var item = _store.Wishlist
                .FirstOrDefault(w => w.ProductId == productId);

            if (item != null)
            {
                _store.Wishlist.Remove(item);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}