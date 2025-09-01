using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecureLoginRoleApp.Data;
using SecureLoginRoleApp.Models;

var builder = WebApplication.CreateBuilder(args);

// -------------------- DATABASE --------------------
// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -------------------- IDENTITY --------------------
// Add Identity with Roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// -------------------- COOKIE SETTINGS --------------------
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// -------------------- CONTROLLERS --------------------
builder.Services.AddControllersWithViews();

// -------------------- BUILD APP --------------------
var app = builder.Build();

// -------------------- CREATE ROLES --------------------
async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roleNames = { "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    // Default Admin
    var adminUser = new ApplicationUser { UserName = "admin", Email = "admin@test.com", EmailConfirmed = true };
    string adminPassword = "Admin@123";
    if (await userManager.FindByNameAsync(adminUser.UserName) == null)
    {
        var createUser = await userManager.CreateAsync(adminUser, adminPassword);
        if (createUser.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    // Default User
    var normalUser = new ApplicationUser { UserName = "user1", Email = "user1@test.com", EmailConfirmed = true };
    string userPassword = "User@123";
    if (await userManager.FindByNameAsync(normalUser.UserName) == null)
    {
        var createUser = await userManager.CreateAsync(normalUser, userPassword);
        if (createUser.Succeeded)
            await userManager.AddToRoleAsync(normalUser, "User");
    }
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRoles(services);
}

// -------------------- MIDDLEWARE --------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Force HTTPS on default port 443
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // must come before Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
