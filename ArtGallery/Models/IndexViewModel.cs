using ArtGallery.Models.Structs.Dto;
using ArtGallery.Models.Structs.Entity;

namespace ArtGallery.Models;

public class IndexViewModel
{
    public IEnumerable<ArtDto> Arts { get; set; }
    
    public IEnumerable<GenreDto> Genres { get; set; }
}