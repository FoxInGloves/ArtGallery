using ArtGallery.Models.Structs.Dto;
using ArtGallery.Models.Structs.Entity;

namespace ArtGallery.Models;

public class MainPageViewModel
{
    public IEnumerable<GenreDto> Genres { get; set; }
    
    public IEnumerable<Art> Arts { get; set; }
}