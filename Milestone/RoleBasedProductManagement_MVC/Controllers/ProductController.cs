// ProductController.cs
// ==========================================================================
// Here we are defining the ProductController for managing products.
// This controller will handle CRUD operations for products (Create, Read, Update, Delete).
// It uses dependency injection to get the ApplicationDbContext (for database access)
// and IDataProtector (for encrypting/decrypting sensitive data like product price).
//
// Key Features Implemented:
// - Role-based Authorization using [Authorize] attribute.
// - Admin: Full control (Create, Edit, Delete).
// - Manager: Limited control (Edit, View only).
// - CSRF protection using [ValidateAntiForgeryToken].
// - Data Protection API for securing product prices in the database.
// - Success/Error messages using TempData for better user feedback.
// ==========================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoleBasedProductManagement_MVC.Data;
using RoleBasedProductManagement_MVC.Models;

namespace RoleBasedProductManagement_MVC.Controllers
{
    // Apply authorization for Admin & Manager by default
    [Authorize(Roles = "Admin,Manager")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;   // Database context
        private readonly IDataProtector _protector;       // For encrypting/decrypting prices

        // Constructor: Inject ApplicationDbContext and Data Protection Provider
        public ProductController(ApplicationDbContext context, IDataProtectionProvider provider)
        {
            _context = context;
            _protector = provider.CreateProtector("ProductPriceProtector"); // Unique key for price protection
        }

        // ======================================================================
        // GET: /Product
        // Displays the list of all products.
        // Price is decrypted before showing to the user.
        // ======================================================================
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();

            // Decrypt prices before sending to the view
            foreach (var p in products)
            {
                if (!string.IsNullOrEmpty(p.EncryptedPrice))
                {
                    p.Price = decimal.Parse(_protector.Unprotect(p.EncryptedPrice));
                }
            }

            return View("ProductList", products);
        }

        // ======================================================================
        // GET: /Product/Create
        // Only Admin can access this action.
        // Displays the CreateProduct form.
        // ======================================================================
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View("CreateProduct");
        }

        // ======================================================================
        // POST: /Product/Create
        // Only Admin can create new products.
        // Encrypts price before saving into the database.
        // ======================================================================
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF protection
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name,Price")] Product product)
        {
            // Manually check ModelState and log errors if any
            if (!ModelState.IsValid)
            {
                // Optional: Debugging helper
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine(string.Join(", ", errors));
                return View("CreateProduct", product);
            }

            // Encrypt and save
            product.EncryptedPrice = _protector.Protect(product.Price.ToString());
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Product \"{product.Name}\" has been successfully created!";
            return RedirectToAction(nameof(Index));
        }

        // ======================================================================
        // GET: /Product/Edit/{id}
        // Admin and Manager both can edit products.
        // Decrypts price before showing it in the edit form.
        // ======================================================================
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            // Decrypt price before editing
            if (!string.IsNullOrEmpty(product.EncryptedPrice))
            {
                product.Price = decimal.Parse(_protector.Unprotect(product.EncryptedPrice));
            }

            return View("EditProduct", product);
        }

        // ======================================================================
        // POST: /Product/Edit/{id}
        // Admin and Manager both can update products.
        // Encrypts the updated price before saving to the database.
        // ======================================================================
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF protection
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit([Bind("Id,Name,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                var dbProduct = await _context.Products.FindAsync(product.Id);
                if (dbProduct == null) return NotFound();

                dbProduct.Name = product.Name;

                if (product.Price > 0)
                {
                    dbProduct.EncryptedPrice = _protector.Protect(product.Price.ToString()); // Encrypt updated price
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Product \"{product.Name}\" has been successfully updated!";
                return RedirectToAction(nameof(Index));
            }
            return View("EditProduct", product);
        }

        // ======================================================================
        // GET: /Product/Delete/{id}
        // Only Admin can delete products.
        // ======================================================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["ErrorMessage"] = $"Product \"{product.Name}\" has been deleted!";
            return RedirectToAction(nameof(Index));
        }
    }
}
