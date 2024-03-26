using ArtGallery.Models.Structs.Dto;
using ArtGallery.Models.Structs.Entity;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Data;

public class Repository
{
    private readonly ApplicationDbContext _context;
    
    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public virtual async Task<IEnumerable<Art>> GetArtsAsync()
    {
        return await _context.Arts.ToListAsync();
    }

    public virtual async Task<Art?> GetArtAsync(string artId)
    {
        return await _context.Arts.FirstOrDefaultAsync(x => x.Id == artId);
    }

    public virtual async Task UpdateArtAsync(string originalArtId, ArtDto updateArt)
    {
        var art = await _context.Arts.FindAsync(originalArtId);

        if (art != null)
        {
            art.Id = originalArtId;
            art.Name = updateArt.Name;
            art.Size = updateArt.Size;
            art.Price = updateArt.Price;
            art.Description = updateArt.Description;
            art.ArtistId = updateArt.ArtistId;
            art.GenreId = updateArt.GenreId;
            art.IconPath = updateArt.IconPath;
            art.DateOfCreation = updateArt.DateOfCreation;
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task CreateArtAsync(ArtDto newArt)
    {
        var art = new Art
        {
            Id = Guid.NewGuid().ToString(),
            Name = newArt.Name,
            Size = newArt.Size,
            Price = newArt.Price,
            Description = newArt.Description,
            ArtistId = newArt.ArtistId,
            GenreId = newArt.GenreId,
            IconPath = newArt.IconPath,
            DateOfCreation = newArt.DateOfCreation
        };

        await _context.Arts.AddAsync(art);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteArtAsync(string artId)
    {
        var art = await _context.Arts.FindAsync(artId);
        if (art != null) _context.Arts.Remove(art);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<Artist>> GetArtistsAsync()
    {
        return await _context.Artists.ToListAsync();
    }
    
    public virtual async Task<Artist?> GetArtistAsync(string artistId)
    {
        return await _context.Artists.FindAsync(artistId);
    }
    
    public virtual async Task UpdateArtistAsync(string originalArtistId, ArtistDto updateArtist)
    {
        var artist = await _context.Artists.FindAsync(originalArtistId);
        if (artist != null)
        {
            artist.Id = originalArtistId;
            artist.Country = updateArtist.Country;
            artist.Description = updateArtist.Description;
            artist.IconPath = updateArtist.IconPath;
            artist.Name = updateArtist.Name;
        }
        await _context.SaveChangesAsync();
    }
    
    public async Task CreateArtistAsync(ArtistDto newArtist)
    {
        var artist = new Artist
        {
            Id = Guid.NewGuid().ToString(),
            Country = newArtist.Country,
            Name = newArtist.Name,
            Description = newArtist.Description,
            IconPath = newArtist.IconPath,
        };

        await _context.Artists.AddAsync(artist);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteArtistAsync(string artistId)
    {
        var artist = await _context.Artists.FindAsync(artistId);
        if (artist != null) _context.Artists.Remove(artist);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<Genre>> GetGenresAsync()
    {
        return await _context.Genres.ToListAsync();
    }
}