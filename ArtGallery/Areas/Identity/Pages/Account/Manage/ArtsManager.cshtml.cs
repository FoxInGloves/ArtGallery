using System.ComponentModel.DataAnnotations;
using ArtGallery.Data;
using ArtGallery.Models.Structs.Dto;
using ArtGallery.Models.Structs.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace ArtGallery.Areas.Identity.Pages.Account.Manage;

public class ArtsManagerModel : PageModel
{
    private readonly Repository _repository;
    private readonly ILogger<ArtsManagerModel> _logger;
    
    public ArtsManagerModel(Repository repository, ILogger<ArtsManagerModel> logger)
    {
        _repository = repository;
        _logger = logger;
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
        var arts = await _repository.GetArtsAsync();
        
        Arts = arts.Select(art => new ArtDto()
        {
            Id = art.Id,
            Name = art.Name,
            Description = art.Description,
            IconPath = art.IconPath,
            Size = art.Size,
            Price = art.Price,
            ArtistId = art.ArtistId,
        });
        
        Artists = await _repository.GetArtistsAsync();
        Genres = await _repository.GetGenresAsync();

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
        var selectedArt = await _repository.GetArtAsync(artId);
        var selectedArtist = await _repository.GetArtistAsync(artistId);
        await LoadAsync(selectedArt, selectedArtist);
        return Page();
    }
    
    public Task<IActionResult> OnPostFindContentAsync(string artId, string artistId)
    {
        return Task.FromResult<IActionResult>(RedirectToAction(null, new {artId, artistId}));
    }

    public async Task<IActionResult> OnPostUpdateArtAsync(IFormFile? picture)
    {
        //TODO add ModelState.IsValid
        var iconPath = SelectedArt.IconPath;
        var tempIconPath = iconPath;

        if (picture?.Name != null)
        {
            iconPath = await UploadImageAsync(picture, MethodSelector.Art);
            if (!string.Equals(tempIconPath, iconPath) && iconPath != null)
            {
                RemoveImage(tempIconPath);
            }
        }
        
        var art = new ArtDto
        {
            Id = SelectedArt.Id,
            ArtistId = SelectedArt.ArtistId,
            DateOfCreation = SelectedArt.DateOfCreation,
            Description = SelectedArt.Description,
            GenreId = SelectedArt.GenreId,
            IconPath = iconPath,
            Name = SelectedArt.Name,
            Price = SelectedArt.Price,
            Size = SelectedArt.Size,
        };
        await _repository.UpdateArtAsync(SelectedArt.Id, art);
        _logger.LogInformation("Art '{Id}' has been updated", art.Id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCreateArtAsync(IFormFile? picture)
    {
        string? iconPath;
        if (SelectedArt.IconPath.IsNullOrEmpty())
        {
            iconPath = await UploadImageAsync(picture, MethodSelector.Art);
        }
        else
        {
            iconPath = SelectedArt.IconPath;
        }
        var art = new ArtDto
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
        await _repository.CreateArtAsync(art);
        _logger.LogInformation("Art '{Id}' has been created", art.Id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteArtAsync()
    {
        await _repository.DeleteArtAsync(SelectedArt.Id);
        RemoveImage(SelectedArt.IconPath);
        _logger.LogInformation("Art '{Id}' has been deleted", SelectedArt.Id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateArtistAsync(IFormFile? picture)
    {
        //TODO add ModelState.IsValid
        var iconPath = SelectedArtist.IconPath;
        var tempIconPath = iconPath;
        iconPath = await UploadImageAsync(picture, MethodSelector.Artist);

        if (!string.Equals(tempIconPath, iconPath))
        {
            RemoveImage(tempIconPath);
        }

        var artist = new ArtistDto
        {
            Id = SelectedArtist.Id,
            Country = SelectedArtist.Country,
            Description = SelectedArtist.Description,
            IconPath = iconPath,
            Name = SelectedArtist.Name,
        };
        await _repository.UpdateArtistAsync(SelectedArt.Id, artist);
        _logger.LogInformation("Artist '{Id}' has been updated", artist.Id);
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostCreateArtistAsync(IFormFile? picture)
    {
        string? iconPath;
        if (SelectedArtist.IconPath.IsNullOrEmpty())
        {
            iconPath = await UploadImageAsync(picture, MethodSelector.Artist);
        }
        else
        {
            iconPath = SelectedArtist.IconPath;
        }
        var artist = new ArtistDto
        {
            Id = Guid.NewGuid().ToString(),
            Country = SelectedArtist.Country,
            Description = SelectedArtist.Description,
            IconPath = iconPath,
            Name = SelectedArtist.Name,
        };
        await _repository.CreateArtistAsync(artist);
        _logger.LogInformation("Artist '{Id}' has been created", artist.Id);
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostDeleteArtistAsync()
    {
        await _repository.DeleteArtistAsync(SelectedArtist.Id);
        RemoveImage(SelectedArtist.IconPath);
        _logger.LogInformation("Artist '{Id}' has been deleted", SelectedArtist.Id);
        return RedirectToPage();
    }
    
    private static async Task<string?> UploadImageAsync(IFormFile? image, MethodSelector selector)
    {
        string? imagePath;
        if (image != null)
        {
            var fileName = Path.GetFileName(image.FileName);
            var uploadPath = $"{Directory.GetCurrentDirectory()}/wwwroot/images/{Methods[selector]}/{fileName}";
            if (System.IO.File.Exists(uploadPath))
            {
                uploadPath = GenerateUniqueFileName(fileName, selector);
            }
            await using var fileStream = new FileStream(uploadPath, FileMode.Create);
            await image.CopyToAsync(fileStream);
            imagePath = $"/images/{Methods[selector]}/{fileName}";
        }
        else
        {
            imagePath = null;
        }
        return imagePath;
    }
    
    private static string GenerateUniqueFileName(string fileName, MethodSelector selector)
    {
        var extension = Path.GetExtension(fileName);
        var fileNameOnly = Path.GetFileNameWithoutExtension(fileName);

        var counter = 1;
        var uniqueFileName = fileName;
        
        while (System.IO.File.Exists($"{Directory.GetCurrentDirectory()}/wwwroot/images/{Methods[selector]}/{uniqueFileName}"))
        {
            var tempFileName = $"{Directory.GetCurrentDirectory()}/wwwroot/images/{Methods[selector]}/{fileNameOnly}({counter}){extension}";
            uniqueFileName = tempFileName;
            counter++;
        }
        return uniqueFileName;
    }

    private static void RemoveImage(string? iconPath)
    {
        if (iconPath == null) return;
        var uploadPath = $"{Directory.GetCurrentDirectory()}/wwwroot/{iconPath}";
        if (System.IO.File.Exists(uploadPath))
        {
            System.IO.File.Delete(uploadPath);
        }
    }
    
    private enum MethodSelector
    {
        Art,
        Artist
    }

    private static readonly Dictionary<MethodSelector, string> Methods = new Dictionary<MethodSelector, string>()
    {
        { MethodSelector.Art, "arts" },
        { MethodSelector.Artist, "artists" }
    };
}