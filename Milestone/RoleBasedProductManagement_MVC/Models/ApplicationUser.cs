// Here we have the ApplicationUser class which extends IdentityUser
// Its shows how to add custom properties to the IdentityUser class

using Microsoft.AspNetCore.Identity;  //This is the namespace for Identity framework

namespace RoleBasedProductManagement_MVC.Models  // This namespace contains the models for the application
{
    // Extending IdentityUser to add Role
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }  // Admin / Manager //This will help in role-based authorization
    }
}
