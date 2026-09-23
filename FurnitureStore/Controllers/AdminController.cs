using FurnitureStore.Services;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly FurnitureShop _store;

        // Temporary prototype credentials
        // Change these before presenting the application.
        private const string AdminUsername = "admin";
        private const string AdminPassword = "Admin123!";

        public AdminController(FurnitureShop store)
        {
            _store = store;
        }

        // -----------------------------
        // ADMIN LOGIN
        // -----------------------------

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string username,
            string password)
        {
            if (username == AdminUsername &&
                password == AdminPassword)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        // -----------------------------
        // ADMIN LOGOUT
        // -----------------------------

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        // -----------------------------
        // ADMIN DASHBOARD
        // -----------------------------

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_store.Products);
        }

        // -----------------------------
        // CREATE PRODUCT
        // -----------------------------

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _store.Categories;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _store.Categories;
                return View(product);
            }

            product.Id = _store.Products.Any()
                ? _store.Products.Max(p => p.Id) + 1
                : 1;

            var category = _store.Categories
                .FirstOrDefault(c => c.Id == product.CategoryId);

            if (category != null)
            {
                product.CategoryName = category.Name;
            }

            _store.Products.Add(product);

            return RedirectToAction(nameof(Index));
        }

        // -----------------------------
        // EDIT PRODUCT
        // -----------------------------

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _store.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _store.Categories;

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _store.Categories;
                return View(product);
            }

            var existingProduct = _store.Products
                .FirstOrDefault(p => p.Id == id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.IsFeatured = product.IsFeatured;

            var category = _store.Categories
                .FirstOrDefault(c => c.Id == product.CategoryId);

            if (category != null)
            {
                existingProduct.CategoryName = category.Name;
            }

            return RedirectToAction(nameof(Index));
        }

        // -----------------------------
        // DELETE PRODUCT
        // -----------------------------

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _store.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _store.Products
                .FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                _store.Products.Remove(product);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}