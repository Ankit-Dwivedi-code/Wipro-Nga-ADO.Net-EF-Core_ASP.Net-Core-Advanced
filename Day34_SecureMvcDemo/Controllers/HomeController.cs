using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Day34_SecureMvcDemo.Models;

namespace Day34_SecureMvcDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Safeecho
    // public IActionResult SafeEcho(string input)
    // {// this will implement validation( basic length check )
    //     if (string.IsNullOrWhiteSpace(Request.Form["input"]))
    //     {
    //         ModelState.AddModelError("input", "Input cannot be empty");
    //     }
    //     else if (Request.Form["input"].ToString().Length > 100)//maximum length
    //     {
    //         ModelState.AddModelError("input", "Input cannot be longer than 100 characters");
    //     }
    //     if (!ModelState.IsValid) // here we are checking if the model state is valid
    //     {
    //         ViewBag.Input = Request.Form["input"]; // Store the input in ViewBag for redisplay
    //         return View();
    //     }
    //     //razor view by default do output encoding --> prevents XSS attacks ie
    //     // <input value="@Request.Form["input"]" />
    //     //xss stands for cross-site scripting In web applications, it is a security vulnerability that allows 
    //     // an attacker to inject malicious scripts into content that is then served to other users.
    //     ViewBag.Input = Request.Form["input"];
    //     return View();
    // }

    // SafeEcho action
[HttpGet]
public IActionResult SafeEcho()
{
    // Just render the view on GET
    return View();
}

[HttpPost]
public IActionResult SafeEcho(string input)
{
    // Basic validation
    if (string.IsNullOrWhiteSpace(input))
    {
        ModelState.AddModelError("input", "Input cannot be empty");
    }
    else if (input.Length > 100) // maximum length
    {
        ModelState.AddModelError("input", "Input cannot be longer than 100 characters");
    }

    // Store input in ViewBag so it can be redisplayed in the view
    ViewBag.Input = input;

    return View();
}


    
}
