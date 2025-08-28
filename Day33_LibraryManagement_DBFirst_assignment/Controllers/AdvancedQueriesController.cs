using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Day33_LibraryManagement_DBFirst_assignment.Models;

namespace Day33_LibraryManagement_DBFirst_assignment.Controllers
{
    public class AdvancedQueriesController : Controller
    {
        private readonly LibraryDbAssignmentContext _context;

        public AdvancedQueriesController(LibraryDbAssignmentContext context)
        {
            _context = context;
        }

        // GET: AdvancedQueries/BooksWithAuthorsAndGenres
        public async Task<IActionResult> BooksWithAuthorsAndGenres()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.GenresGenres)
                .ToListAsync();

            return View(books);
        }

        // Example of a filtered query: All books in a specific genre
        public async Task<IActionResult> BooksByGenre(string genreName)
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.GenresGenres)
                .Where(b => b.GenresGenres.Any(g => g.Name == genreName))
                .ToListAsync();

            ViewBag.Genre = genreName;
            return View(books);
        }

        // Example of performance query: Top N authors by book count
        public async Task<IActionResult> TopAuthors(int top = 3)
        {
            var authors = await _context.Authors
                .Select(a => new
                {
                    a.Name,
                    BookCount = a.Books.Count
                })
                .OrderByDescending(a => a.BookCount)
                .Take(top)
                .ToListAsync();

            return Json(authors); // returns JSON result
        }
    }
}
