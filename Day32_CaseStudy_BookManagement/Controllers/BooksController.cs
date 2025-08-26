using Microsoft.AspNetCore.Mvc;
using Day32_CaseStudy_BookManagement.Data;
using Day32_CaseStudy_BookManagement.Models;
using System.Threading.Tasks;

namespace Day32_CaseStudy_BookManagement.Controllers
{
    public class BooksController : Controller
    {
        private readonly BooksRepository _booksRepository;

        public BooksController(BooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        // Index: Display all books and search
        public async Task<IActionResult> Index(string searchQuery)
        {
            var books = string.IsNullOrEmpty(searchQuery)
                ? await _booksRepository.GetAllBooksAsync()
                : await _booksRepository.SearchBooksAsync(searchQuery);

            ViewBag.SearchQuery = searchQuery;
            return View(books);
        }

        // Details: Display book by id
        public async Task<IActionResult> Details(int id)
        {
            var book = await _booksRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // Create: GET
        public IActionResult Create()
        {
            return View();
        }

        // Create: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                await _booksRepository.AddBookAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }
    }
}
