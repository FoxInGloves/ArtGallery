namespace ArtGallery.Models.Structs;

public class Art(string artName, string? artPrice, string author)
{
    public string? ArtName { get; set; } = artName;

    public string? ArtPrice { get; set; } = artPrice;

    public string? Author { get; set; } = author;
}