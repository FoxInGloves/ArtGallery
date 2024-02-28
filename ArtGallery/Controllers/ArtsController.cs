using System.Diagnostics;
using ArtGallery.Models;
using ArtGallery.Models.Structs;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers;

public class ArtsController : Controller
{
    private readonly ILogger<ArtsController> _logger;

    public ArtsController(ILogger<ArtsController> logger)
    {
        _logger = logger;
    }
    
    public IActionResult Index()
    {
        var arts = GetArts();
        
        return View(arts);
    }
//1535px max width
    
    public IActionResult Artist(string idArt)
    {
        return View();
    }

    public IActionResult Picture()
    {
        return View();
    }

    public IActionResult Main()
    {
        var arts = GetArts(12);
        
        return View(arts);
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    private IEnumerable<Art> GetArts()
    {
        var arts = new List<Art>();
        
        for (var i = 0; i < 35; i++)
        {
            arts.Add(new Art
            {
                Name = "9 вал", 
                Price = "1000$", 
                Author = "Иван Айвазовский"
            });
        }

        return arts;
    }

    private IEnumerable<Art> GetArts(int count)
    {
        var arts = new List<Art>();

        for (var i = 0; i < count; i++)
        {
            arts.Add(new Art
            {
                Name = "9 вал", 
                Price = "1000$", 
                Author = "Иван Айвазовский"
            });
        }

        return arts;
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}