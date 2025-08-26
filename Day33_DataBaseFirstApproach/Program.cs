using Day33_DataBaseFirstApproach.Models;

// Steps to making this
// Create a project
// Add NuGet packages: Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Tools
// Scaffold the database
// Create a DbContext and models
// dotnet ef dbcontext scaffold "Data Source=HP\SQLEXPRESS;Initial Catalog=Day32_databaseFirstApproachDB;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models
// The generated models will be placed in the Models folder
// Use the DbContext to display data
// You have to create database, tables and seed data first

using var context = new Day32DatabaseFirstApproachDbContext();

// Fetch Students
Console.WriteLine("Students:");
foreach (var s in context.Students.ToList())
{
    Console.WriteLine($"{s.StudentId} - {s.Name} - {s.Email}");
}

// Fetch Trainers
Console.WriteLine("\nTrainers:");
foreach (var t in context.Trainers.ToList())
{
    Console.WriteLine($"{t.TrainerId} - {t.Name}");
}

// Fetch Courses
Console.WriteLine("\nCourses:");
foreach (var c in context.Courses.ToList())
{
    Console.WriteLine($"{c.CourseId} - {c.Title} - TrainerId: {c.TrainerId}");
}
