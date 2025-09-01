// Here we have the ApplicationDbContext class which extends IdentityDbContext
// It shows how to configure the database context for the application

using Microsoft.AspNetCore.Identity.EntityFrameworkCore; //This is the namespace for Identity framework
using Microsoft.EntityFrameworkCore; //This is the namespace for Entity Framework Core
using RoleBasedProductManagement_MVC.Models; // This is the namespace for the application models

namespace RoleBasedProductManagement_MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //This class represents the database context for the application
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) //This constructor takes DbContextOptions as parameter
            : base(options) { }

        // Products Table
        public DbSet<Product> Products { get; set; } //Here we define the Products table
    }
}
