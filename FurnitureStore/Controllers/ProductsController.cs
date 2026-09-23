using FurnitureStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly FurnitureShop _store;

        public ProductsController(FurnitureShop store)
        {
            _store = store;
        }

        public IActionResult Index(int? categoryId)
        {
            ViewBag.Categories = _store.Categories;

            var products = _store.Products.AsEnumerable();

            if (categoryId.HasValue)
            {
                products = products
                    .Where(p => p.CategoryId == categoryId.Value);

                var category = _store.Categories
                    .FirstOrDefault(c => c.Id == categoryId.Value);

                ViewBag.CurrentCategory = category?.Name;
            }

            return View(products.ToList());
        }

        public IActionResult Details(int id)
        {
            var product = _store.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}