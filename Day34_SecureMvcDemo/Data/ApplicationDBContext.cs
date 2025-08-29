// We will be using this file for database interactions
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Day34_SecureMvcDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    // IdentityDBContext will provide the necessary tables for user management like authentication and role management
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define your DbSets (tables) here
    }
}
