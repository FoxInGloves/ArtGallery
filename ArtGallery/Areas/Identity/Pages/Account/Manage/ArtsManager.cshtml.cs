using System.ComponentModel.DataAnnotations;
using ArtGallery.Data;
using ArtGallery.Data.Abstractions;
using ArtGallery.Data.Implementations;
using ArtGallery.Models.Services;
using ArtGallery.Models.Structs.Dto;
using ArtGallery.Models.Structs.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace ArtGallery.Areas.Identity.Pages.Account.Manage;

public class ArtsManagerModel : PageModel
{
    private readonly ILogger<ArtsManagerModel> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ConverterToDto _converterToDto;
    private readonly ImageManipulation _imageManipulation;
    
    public ArtsManagerModel(
        ILogger<ArtsManagerModel> logger, 
        IUnitOfWork unitOfWork, 
        ImageManipulation imageManipulation)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _imageManipulation = imageManipulation;
        _converterToDto = new ConverterToDto();
    }
    
    [TempData]
    public string StatusMessage { get; set; }
    
    public IEnumerable<ArtDto> Arts { get; set; }
    
    public IEnumerable<Artist> Artists { get; set; }
    
    public IEnumerable<Genre> Genres { get; set; }
    
    [BindProperty]
    public SelectedArtModel SelectedArt { get; set; }
    
    [BindProperty]
    public SelectedArtistModel SelectedArtist { get; set; }
    
    public class SelectedArtModel
    {
        public string Id { get; set; }
        
        public string? ArtistId { get; set; }
        
        public string? DateOfCreation { get; set; }
        
        public string? Description { get; set; }
         
        public string? GenreId { get; set; }
        
        public string? IconPath { get; set; }
        
        [Required]
        public string? Name { get; set; }
        
        public string? Price { get; set; }
        
        public string? Size { get; set; }
    }
    
    public class SelectedArtistModel
    {
        public string Id { get; set; }
        
        public string? Country { get; set; }
        
        public string? Description { get; set; }
        
        public string? IconPath { get; set; }
        
        public string? Name { get; set; }
    }

    public async Task LoadAsync(Art? selectedArt, Artist? selectedArtist)
    {
        var arts = await _unitOfWork.ArtRepository.GetAsync();
        
        Arts = arts.Select(art => _converterToDto.Convert(art));

        Artists = await _unitOfWork.ArtistRepository.GetAsync();
        Genres = await _unitOfWork.GenresRepository.GetAsync();

        if (selectedArt == null)
        {
            SelectedArt = new SelectedArtModel();
        }
        else
        {
            SelectedArt = new SelectedArtModel
            {
                Id = selectedArt.Id,
                ArtistId = selectedArt.ArtistId,
                DateOfCreation = selectedArt.DateOfCreation,
                Description = selectedArt.Description,
                GenreId = selectedArt.GenreId,
                IconPath = selectedArt.IconPath,
                Name = selectedArt.Name,
                Price = selectedArt.Price,
                Size = selectedArt.Size
            };
        }

        if (selectedArtist == null)
        {
            SelectedArtist = new SelectedArtistModel();
        }
        else
        {
            SelectedArtist = new SelectedArtistModel()
            {
                Country = selectedArtist.Country,
                Description = selectedArtist.Description,
                IconPath = selectedArtist.IconPath,
                Id = selectedArtist.Id,
                Name = selectedArtist.Name
            };
        }
    }
    
    public async Task<IActionResult> OnGetAsync(string artId, string artistId)
    {
        var selectedArt = await _unitOfWork.ArtRepository.GetByIdAsync(artId);
        var selectedArtist = await _unitOfWork.ArtistRepository.GetByIdAsync(artistId);
        await LoadAsync(selectedArt, selectedArtist);
        return Page();
    }
    
    public Task<IActionResult> OnPostFindContentAsync(string artId, string artistId)
    {
        return Task.FromResult<IActionResult>(RedirectToAction(null, new {artId, artistId}));
    }

    public async Task<IActionResult> OnPostUpdateArtAsync(IFormFile? picture)
    {
        var iconPath = await _imageManipulation.GetIconPathAsync(picture, SelectedArt.IconPath);
        
        await UpdateArtAsync(SelectedArt.Id, iconPath);
        _logger.LogInformation("Art '{Id}' has been updated", SelectedArt.Id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCreateArtAsync(IFormFile? picture)
    {
        string? iconPath;
        if (SelectedArt.IconPath.IsNullOrEmpty())
        {
            iconPath = await _imageManipulation.UploadImageAsync(picture, ImageManipulation.MethodSelector.Art);
        }
        else
        {
            iconPath = SelectedArt.IconPath;
        }
        var art = new Art
        {
            Id = Guid.NewGuid().ToString(),
            ArtistId = SelectedArt.ArtistId,
            DateOfCreation = SelectedArt.DateOfCreation,
            Description = SelectedArt.Description,
            GenreId = SelectedArt.GenreId,
            IconPath = iconPath,
            Name = SelectedArt.Name,
            Price = SelectedArt.Price,
            Size = SelectedArt.Size,
        };
        await _unitOfWork.ArtRepository.CreateAsync(art);
        await _unitOfWork.SaveAsync();
        _logger.LogInformation("Art '{Id}' has been created", art.Id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteArtAsync()
    {
        await _unitOfWork.ArtRepository.DeleteAsync(SelectedArt.Id);
        await _unitOfWork.SaveAsync();
        _imageManipulation.RemoveImage(SelectedArt.IconPath);
        _logger.LogInformation("Art '{Id}' has been deleted", SelectedArt.Id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateArtistAsync(IFormFile? picture)
    {
        //TODO add ModelState.IsValid
        var iconPath = await _imageManipulation.GetIconPathAsync(picture, SelectedArtist.IconPath);
        
        await UpdateArtistAsync(SelectedArtist.Id, iconPath);
        _logger.LogInformation("Artist '{Id}' has been updated", SelectedArtist.Id);
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostCreateArtistAsync(IFormFile? picture)
    {
        string? iconPath;
        if (SelectedArtist.IconPath.IsNullOrEmpty())
        {
            iconPath = await _imageManipulation.UploadImageAsync(picture, ImageManipulation.MethodSelector.Artist);
        }
        else
        {
            iconPath = SelectedArtist.IconPath;
        }
        
        var artist = new Artist
        {
            Id = Guid.NewGuid().ToString(),
            Country = SelectedArtist.Country,
            Description = SelectedArtist.Description,
            IconPath = iconPath,
            Name = SelectedArtist.Name,
        };
        
        await _unitOfWork.ArtistRepository.CreateAsync(artist);
        await _unitOfWork.SaveAsync();
        
        _logger.LogInformation("Artist '{Id}' has been created", artist.Id);
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostDeleteArtistAsync()
    {
        await _unitOfWork.ArtistRepository.DeleteAsync(SelectedArtist.Id);
        await _unitOfWork.SaveAsync();
        _imageManipulation.RemoveImage(SelectedArtist.IconPath);
        _logger.LogInformation("Artist '{Id}' has been deleted", SelectedArtist.Id);
        return RedirectToPage();
    }

    private async Task UpdateArtAsync(string idOriginalArt, string? iconPath)
    {
        var art = await _unitOfWork.ArtRepository.GetByIdAsync(idOriginalArt);
        if(art == null) 
            return;
        
        MappingArt(ref art, iconPath);
        await _unitOfWork.ArtRepository.UpdateAsync(art);
        await _unitOfWork.SaveAsync();
    }

    private void MappingArt(ref Art art, string? newIconPath)
    {
        art.ArtistId = SelectedArt.ArtistId;
        art.DateOfCreation = SelectedArt.DateOfCreation;
        art.Description = SelectedArt.Description;
        art.GenreId = SelectedArt.GenreId;
        art.IconPath = newIconPath;
        art.Name = SelectedArt.Name;
        art.Price = SelectedArt.Price;
        art.Size = SelectedArt.Size;
    }

    private async Task UpdateArtistAsync(string idOriginalArtist, string? iconPath)
    {
        var artist = await _unitOfWork.ArtistRepository.GetByIdAsync(idOriginalArtist);
        if(artist == null)
            return;
        
        MappingArtist(ref artist, iconPath);
        await _unitOfWork.ArtistRepository.UpdateAsync(artist);
        await _unitOfWork.SaveAsync();
    }

    private void MappingArtist(ref Artist artist, string? iconPath)
    {
        artist.Country = SelectedArtist.Country;
        artist.Description = SelectedArtist.Description;
        artist.IconPath = iconPath;
        artist.Name = SelectedArtist.Name;
    }
}