using System.ComponentModel.DataAnnotations;
using ArtGallery.Data;
using ArtGallery.Models.Structs.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtGallery.Areas.Identity.Pages.Account.Manage;

public class ArtsManager : PageModel
{
    private readonly Repository _repository;
    
    public ArtsManager(Repository repository)
    {
        _repository = repository;
    }
    
    [TempData]
    public string StatusMessage { get; set; }
    
    public IEnumerable<Art> Arts { get; set; }
    
    public IEnumerable<Artist> Artists { get; set; }
    
    public IEnumerable<Genre> Genres { get; set; }
    
    [BindProperty]
    public SelectedArtModel SelectedArt { get; set; }
    
    [BindProperty]
    public SelectedArtistModel SelectedArtist { get; set; }
    
    public class SelectedArtModel
    {
        //public string Id { get; set; }
        [Required]
        public string? Name { get; set; }
        
        public string? Price { get; set; }
        
        public string? Size { get; set; }
        
        public string? DateOfCreation { get; set; }
        
        public string? Description { get; set; }
        
        [Required]
        public string? IconPath { get; set; }
        
        public string? GenreId { get; set; }
        
        public string? ArtistId { get; set; }
    }
    
    public class SelectedArtistModel
    {
        public string? Name { get; set; }
        
        public string? Description { get; set; }
        
        public string? IconPath { get; set; }
    }

    public async Task LoadAsync(Art? selectedArt)
    {
        Arts = await _repository.GetArtsAsync();

        Artists = await _repository.GetArtistsAsync();

        Genres = await _repository.GetGenresAsync();

        if (selectedArt == null)
        {
            SelectedArt = new SelectedArtModel();
        }
        else
        {
            SelectedArt = new SelectedArtModel()
            {
                ArtistId = selectedArt.ArtistId,
                DateOfCreation = selectedArt.DateOfCreation,
                Description = selectedArt.Description,
                GenreId = selectedArt.GenreId,
                IconPath = selectedArt.IconPath,
                //Id = selectedArt.Id,
                Name = selectedArt.Name,
                Price = selectedArt.Price,
                Size = selectedArt.Size
            };
        }
        
        SelectedArtist = new SelectedArtistModel();
    }
    
    public async Task<IActionResult> OnGetAsync(string artId)
    {
        var selectedArt = await _repository.GetArtAsync(artId);
        await LoadAsync(selectedArt);
        return Page();
    }

    public async Task OnPostUpdateArtAsync(string artId)
    {
        var art = _repository.GetArtAsync(artId);

        //TODO add ModelState.IsValid
        
    }

    public Task<IActionResult> OnPostFindArtAsync(string artId)
    {
        return Task.FromResult<IActionResult>(RedirectToAction(null, new {artId}));
    }
}