using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Day33_LibraryManagement_DBFirst_assignment.Models;

    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext (DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Day33_LibraryManagement_DBFirst_assignment.Models.Author> Author { get; set; } = default!;

public DbSet<Day33_LibraryManagement_DBFirst_assignment.Models.Book> Book { get; set; } = default!;

public DbSet<Day33_LibraryManagement_DBFirst_assignment.Models.Genre> Genre { get; set; } = default!;
    }
