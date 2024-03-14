using System.Diagnostics;
using ArtGallery.Models;
using ArtGallery.Models.Structs.Dto;
using ArtGallery.Models.Structs.Entity;
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
        var viewModel = new MainPageViewModel()
        {
            Genres = GetGenres(5),

            Arts = GetArts(4)
        };
        
        var arts = GetArts(4);
        
        return View(viewModel);
    }
    
    public IActionResult Artist(string idArtist)
    {
        return View();
    }

    public IActionResult Picture()
    {
        return View();
    }

    public IActionResult Catalog()
    {
        var arts = GetArts(35);
        
        return View(arts);
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    private IEnumerable<GenreDto> GetGenres(int count)
    {
        var genres = new List<GenreDto>();

        for (var i = 0; i < count; i++)
        {
            genres.Add(new GenreDto
            {
                Name = "Морской пейзаж",
                IconPath = "~/images/genres/sunset-ocean.png"
            });
        }

        return genres;
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