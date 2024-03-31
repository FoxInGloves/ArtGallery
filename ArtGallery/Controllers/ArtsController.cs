using System.Diagnostics;
using ArtGallery.Models;
using ArtGallery.Models.Structs.Dto;
using ArtGallery.Models.Structs.Entity;
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
            
            Genres = await GetGenres(5),

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

        var artistToView = ConvertArtistToDto(artist);
        
        return View(artistToView);
    }

    public async Task<IActionResult> Picture(string artId)
    {
        var art = await _repository.GetArtAsync(artId);

        var artToView = ConvertArtToDto(art);
        
        return View(artToView);
    }

    public async Task<IActionResult> Catalog()
    {
        //TODO подключить репозиторий
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

        var artsToView = ConvertArtToDto(arts);
            
        for (var i = 0; i < count; i++)
        { 
            myarts.AddRange(artsToView); 
        }

        return myarts;
    }

    private async Task<IEnumerable<GenreDto>> GetGenres(int count)
    {
        var myGenres = new List<GenreDto>();

        var genres = await _repository.GetGenresAsync();

        var genresToView = ConvertGenreToDto(genres);

        for (var i = 0; i < count; i++)
        {
            myGenres.AddRange(genresToView);
        }

        return myGenres;
    }

    private IEnumerable<ArtDto> ConvertArtToDto(IEnumerable<Art> originalArts)
    {
        var mappingArts = originalArts.Select(art => new ArtDto()
        {
            Id = art.Id,
            Name = art.Name,
            Description = art.Description,
            IconPath = art.IconPath,
            Size = art.Size,
            Price = art.Price,
            ArtistId = art.ArtistId,
            Artist = art.Artist
        });

        return mappingArts;
    }
    
    private ArtDto ConvertArtToDto(Art originalArt)
    {
        var mappingArts =  new ArtDto()
        {
            Id = originalArt.Id,
            Name = originalArt.Name,
            Description = originalArt.Description,
            IconPath = originalArt.IconPath,
            Size = originalArt.Size,
            Price = originalArt.Price,
            ArtistId = originalArt.ArtistId,
            Artist = originalArt.Artist
        };

        return mappingArts;
    }

    private ArtistDto ConvertArtistToDto(Artist originalArtis)
    {
        var mappingArtist = new ArtistDto
        {
            Name = originalArtis.Name,
            Country = originalArtis.Country,
            Description = originalArtis.Description,
            IconPath = originalArtis.IconPath
        };

        return mappingArtist;
    }

    private IEnumerable<GenreDto> ConvertGenreToDto(IEnumerable<Genre> originalGenres)
    {
        var mappingGenres = originalGenres.Select(genre => new GenreDto()
        {
            Id = genre.Id,
            IconPath = genre.IconPath,
            Name = genre.Name
        });

        return mappingGenres;
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}