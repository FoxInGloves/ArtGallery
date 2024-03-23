using System.Diagnostics;
using ArtGallery.Models;
using ArtGallery.Models.Structs.Dto;
using Microsoft.AspNetCore.Mvc;
using Repository = ArtGallery.Data.Repository;

namespace ArtGallery.Controllers;

public class ArtsController : Controller
{
    private readonly ILogger<ArtsController> _logger;
    private readonly Repository _repository;

    public ArtsController(ILogger<ArtsController> logger, Repository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    public async Task<IActionResult> Index()
    {
        var viewModel = new IndexViewModel()
        {
            Genres = GetGenres(5),

            Arts = await GetArts(4)
        };
        
        return View(viewModel);
    }
    
    public async Task<IActionResult> Artist(string idArtist)
    {
        var artist = await _repository.GetArtistAsync(idArtist);

        if (artist == null)
        {
            return NoContent();
        }

        var model = new ArtistDto
        {
            Name = artist.Name,
            Country = artist.Country,
            Description = artist.Description,
            IconPath = artist.IconPath
        };
        
        return View(artist);
    }

    public IActionResult Picture()
    {
        return View();
    }

    public async Task<IActionResult> Catalog()
    {
        var arts = await GetArts(35);
        
        return View(arts);
    }
    
    public IActionResult Privacy()
    {
        return View();
    }
    
    private async Task<IEnumerable<ArtDto>> GetArts(int count)
    {
        var myarts = new List<ArtDto>();

        
            var arts = await _repository.GetArtsAsync();

            var artDtos = from art in arts
                select new ArtDto()
                {
                    Id = art.Id,
                    Name = art.Name,
                    Description = art.Description,
                    IconPath = art.IconPath,
                    Size = art.Size,
                    Price = art.Price,
                    ArtistId = art.ArtistId,
                    Artist = art.Artist
                };
            
            for (var i = 0; i < count; i++)
            { 
                myarts.AddRange(artDtos); 
            }

        return myarts;
    }

    private IEnumerable<GenreDto> GetGenres(int count)
    {
        var genres = new List<GenreDto>();

        for (var i = 0; i < count; i++)
        {
            genres.Add(new GenreDto
            {
                Name = "Морской пейзаж",
                IconPath = "/images/genres/seascape.png"
            });
        }

        return genres;
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}