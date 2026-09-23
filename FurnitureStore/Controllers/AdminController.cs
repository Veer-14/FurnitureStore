using FurnitureStore.Data;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FurnitureStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly FurnitureDbContext _context;

        // Temporary prototype credentials
        // Change these before the final production deployment.
        private const string AdminUsername = "admin";
        private const string AdminPassword = "Admin123!";

        public AdminController(FurnitureDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // ADMIN LOGIN
        // =====================================================

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            // If the admin is already logged in,
            // send them directly to the dashboard.
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

        // =====================================================
        // ADMIN LOGOUT
        // =====================================================

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        // =====================================================
        // ADMIN DASHBOARD
        // =====================================================

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            return View(products);
        }

        // =====================================================
        // CREATE PRODUCT - GET
        // =====================================================

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories =
                await _context.Categories
                    .OrderBy(c => c.Name)
                    .ToListAsync();

            return View();
        }

        // =====================================================
        // CREATE PRODUCT - POST
        // =====================================================

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories =
                    await _context.Categories
                        .OrderBy(c => c.Name)
                        .ToListAsync();

                return View(product);
            }

            // Make sure the selected category actually exists.
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == product.CategoryId);

            if (category == null)
            {
                ModelState.AddModelError(
                    "CategoryId",
                    "Please select a valid category.");

                ViewBag.Categories =
                    await _context.Categories
                        .OrderBy(c => c.Name)
                        .ToListAsync();

                return View(product);
            }

            // Add the product to PostgreSQL.
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // EDIT PRODUCT - GET
        // =====================================================

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories =
                await _context.Categories
                    .OrderBy(c => c.Name)
                    .ToListAsync();

            return View(product);
        }

        // =====================================================
        // EDIT PRODUCT - POST
        // =====================================================

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories =
                    await _context.Categories
                        .OrderBy(c => c.Name)
                        .ToListAsync();

                return View(product);
            }

            // Make sure the category exists.
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == product.CategoryId);

            if (category == null)
            {
                ModelState.AddModelError(
                    "CategoryId",
                    "Please select a valid category.");

                ViewBag.Categories =
                    await _context.Categories
                        .OrderBy(c => c.Name)
                        .ToListAsync();

                return View(product);
            }

            // Find the existing database record.
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            // Update the database record.
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.IsFeatured = product.IsFeatured;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // DELETE PRODUCT - GET
        // =====================================================

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
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

        // =====================================================
        // DELETE PRODUCT - POST
        // =====================================================

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}