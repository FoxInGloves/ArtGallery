#nullable disable
namespace ArtGallery.Models.Structs.Entity;

public class Art
{
    public string Id { get; set; }
    
    public string Name { get; set; }

    public string Price { get; set; }
    
    public string Description { get; set; }
    
    public string Size { get; set; }
    
    public string DateOfCreation { get; set; }

    public string ArtistId { get; set; }
    
    public string GenreId { get; set; }
    
    public string IconPath { get; set; }
    
    public Artist Artist { get; set; }
}