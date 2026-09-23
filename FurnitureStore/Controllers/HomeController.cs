using FurnitureStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly FurnitureShop _store;

        public HomeController(FurnitureShop store)
        {
            _store = store;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = _store.Categories;

            var featuredProducts = _store.Products
                .Where(p => p.IsFeatured)
                .ToList();

            return View(featuredProducts);
        }
    }
}