using System.Diagnostics;
using ArtGallery.Data.Abstractions;
using ArtGallery.Models;
using ArtGallery.Models.Services;
using ArtGallery.Models.Structs.Dto;
using ArtGallery.Models.Structs.Entity;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers;

public class ArtsController : Controller
{
    private readonly ILogger<ArtsController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ConverterToDto _converterToDto;

    public ArtsController(ILogger<ArtsController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _converterToDto = new ConverterToDto();
    }
    
    public async Task<IActionResult> Index()
    {
        var arts = await _unitOfWork.ArtRepository.GetAsync();
        var genres = await _unitOfWork.GenresRepository.GetAsync();
        
        var viewModel = new IndexViewModel
        {
            //TODO не забыть убрать методы, не относящиеся к бд
            Genres = await GetGenres(genres, 5),
            Arts = await GetArts(arts, 4)
        };
        
        return View(viewModel);
    }
    
    public async Task<IActionResult> Artist(string idArtist)
    {
        var artist = await _unitOfWork.ArtistRepository.GetByIdAsync(idArtist);
        
        if (artist == null)
        {
            return Redirect($"/Arts/Error/");
        }

        var artistToView = _converterToDto.Convert(artist);
        
        return View(artistToView);
    }

    public async Task<IActionResult> Picture(string artId)
    {
        var art = await _unitOfWork.ArtRepository.GetByIdAsync(artId);
        
        if (art == null)
        {
            return Redirect($"/Arts/Error/");
        }

        var artToView = _converterToDto.Convert(art);
        
        return View(artToView);
    }

    public async Task<IActionResult> Catalog()
    {
        var arts = await _unitOfWork.ArtRepository.GetAsync();
        var artsToView = await GetArts(arts, 35);
        
        return View(artsToView);
    }
    
    public IActionResult Privacy()
    {
        return View();
    }
    
    private async Task<IEnumerable<ArtDto>> GetArts(IEnumerable<Art> originalArts, int count)
    {
        var myarts = new List<ArtDto>();
        
        var artsToView = originalArts.Select(x => _converterToDto.Convert(x));
            
        for (var i = 0; i < count; i++)
        { 
            myarts.AddRange(artsToView); 
        }

        return myarts;
    }

    private async Task<IEnumerable<GenreDto>> GetGenres(IEnumerable<Genre> originalGenres, int count)
    {
        var myGenres = new List<GenreDto>();

        var genresToView = originalGenres.Select(x => _converterToDto.Convert(x));

        for (var i = 0; i < count; i++)
        {
            myGenres.AddRange(genresToView);
        }

        return myGenres;
    }
}