// ============================================================================
// Program.cs - Entry point of the ASP.NET Core MVC application
// ----------------------------------------------------------------------------
// This file configures services and middleware required for our application.
// We are implementing a Role-Based Product Management System with ASP.NET Identity.
// Key Features:
// - ASP.NET Identity with Role support (Admin, Manager).
// - Password policy enforced (length, uppercase, special char).
// - SQL Server EF Core integration.
// - Data Protection API for sensitive data encryption (Product Prices).
// - Role seeding (Admin/Manager created automatically if not exist).
// - Secure HTTPS redirection + Access Denied handling.
// ============================================================================

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoleBasedProductManagement_MVC.Data;
using RoleBasedProductManagement_MVC.Models;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// 1. Database Context Configuration
// Using SQL Server as defined in appsettings.json (DefaultConnection).
// This will create tables for Identity (AspNetUsers, AspNetRoles, etc.)
// and custom Product table in our ApplicationDbContext.
// ============================================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ============================================================================
// 2. ASP.NET Identity Configuration
// Here we add Identity with Role support. This ensures we can assign roles
// like "Admin" and "Manager" to users at registration.
// We also configure password policies to match Wipro's requirements.
// ============================================================================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // Users must confirm account (email/phone in real apps)
    options.Password.RequiredLength = 8;           // Minimum 8 characters
    options.Password.RequireUppercase = true;      // At least 1 uppercase
    options.Password.RequireNonAlphanumeric = true;// At least 1 special character
})
.AddEntityFrameworkStores<ApplicationDbContext>() // Identity uses EF Core DB
.AddDefaultTokenProviders()                       // Tokens for reset, confirm, etc.
.AddDefaultUI();                                  // Default Razor Pages for Login/Register

// ============================================================================
// 3. MVC + Razor Pages Support
// Controllers with Views (for Product CRUD + MVC Views)
// Razor Pages (for Identity scaffolding e.g., Register/Login)
// ============================================================================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ============================================================================
// 4. Application Cookie Configuration
// If unauthorized user tries to access a restricted page, they are redirected
// to our custom AccessDenied.cshtml page.
// ============================================================================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Shared/AccessDenied";
});

// ============================================================================
// 5. Data Protection Services
// Used to encrypt/decrypt sensitive data like Product Price.
// ============================================================================
builder.Services.AddDataProtection();

var app = builder.Build();

// ============================================================================
// 6. Role Seeding (Admin / Manager)
// Ensure roles exist in the database when app starts.
// ============================================================================
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await DbInitializer.SeedRolesAsync(roleManager);
}

// ============================================================================
// 7. Middleware Pipeline
// This defines the request handling flow in the app.
// ============================================================================

// Global exception handling + HSTS for production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTP Strict Transport Security
}

app.UseHttpsRedirection();    // Force HTTPS
app.UseStaticFiles();         // Serve CSS, JS, images

app.UseRouting();             // Enable routing system

app.UseAuthentication();      // Enable Authentication (Login/Identity)
app.UseAuthorization();       // Enable Role-based Authorization

// Default MVC route configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Identity Razor Pages (Register/Login/Logout)
app.MapRazorPages();

app.Run();
