using ArtGallery.Data.Abstractions;
using ArtGallery.Models.Structs.Entity;

namespace ArtGallery.Data.Implementations;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
    private GenericRepository<ApplicationUser>? _userRepository;
    private GenericRepository<Art>? _artRepository;
    private GenericRepository<Artist>? _artistRepository;
    private GenericRepository<Genre>? _genreRepository;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

    public IGenericRepository<ApplicationUser> UserRepository => _userRepository ?? new GenericRepository<ApplicationUser>(context);

    public IGenericRepository<Art> ArtRepository => _artRepository ?? new GenericRepository<Art>(context);

    public IGenericRepository<Artist> ArtistRepository => _artistRepository ?? new GenericRepository<Artist>(context);

    public IGenericRepository<Genre> GenresRepository => _genreRepository ?? new GenericRepository<Genre>(context);

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}