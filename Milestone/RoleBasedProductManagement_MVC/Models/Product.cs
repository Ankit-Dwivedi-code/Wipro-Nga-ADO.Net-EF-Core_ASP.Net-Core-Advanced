// // This class represents the Product entity
// // It is used to manage product information in the application

// using System.ComponentModel.DataAnnotations; //This namespace contains classes for data validation

// namespace RoleBasedProductManagement_MVC.Models
// {
//     public class Product
//     {
//         // Here we define the Product entity
//         public int Id { get; set; } //This is the primary key for the Product entity

//         [Required, StringLength(100)] //This specifies that the Name field is required and has a maximum length of 100 characters
//         public string Name { get; set; } //This is the name of the product

//         [Required, DataType(DataType.Currency)] //This specifies that the Price field is required and is displayed as a currency
//         public decimal Price { get; set; } //This is the price of the product
//     }
// }



// Now we will add data protection attributes
// Upper part was for defining the model
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoleBasedProductManagement_MVC.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        // Not mapped to DB, used only in UI
        [NotMapped]
        [Required]
        public decimal Price { get; set; }

        // Stored in DB as encrypted string
        public string EncryptedPrice { get; set; }
    }
}
