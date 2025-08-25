using Microsoft.AspNetCore.Mvc;
using ProductApp.Data;
using ProductApp.Models;

namespace ProductApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductRepository _repo;

        public ProductsController(IConfiguration config)
        {
            _repo = new ProductRepository(config);
        }

        // Show product list
        public IActionResult Index()
        {
            var products = _repo.GetProducts();
            return View(products);
        }

        // Show form
        public IActionResult Create()
        {
            return View();
        }

        // Insert product
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _repo.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // Delete product
        public IActionResult Delete(int id)
        {
            _repo.DeleteProduct(id);
            return RedirectToAction("Index");
        }
    }
}
