using ArtGallery.Models.Structs.Dto;
using ArtGallery.Models.Structs.Entity;

namespace ArtGallery.Models.Services;

public sealed class ConverterToDto
{
    public ArtDto Convert(Art originalArt)
    {
        var mappingArts =  new ArtDto()
        {
            Id = originalArt.Id,
            Name = originalArt.Name,
            Description = originalArt.Description,
            DateOfCreation = originalArt.DateOfCreation,
            IconPath = originalArt.IconPath,
            Size = originalArt.Size,
            Price = originalArt.Price,
            ArtistId = originalArt.ArtistId,
            Artist = originalArt.Artist
        };

        return mappingArts;
    }

    public ArtistDto Convert(Artist originalArtist)
    {
        var mappingArtist = new ArtistDto
        {
            Name = originalArtist.Name,
            Country = originalArtist.Country,
            Description = originalArtist.Description,
            IconPath = originalArtist.IconPath
        };

        return mappingArtist;
    }

    public GenreDto Convert(Genre originalGenre)
    {
        var mappingGenre = new GenreDto
        {
            Id = originalGenre.Id,
            IconPath = originalGenre.IconPath,
            Name = originalGenre.Name
        };

        return mappingGenre;
    }
}