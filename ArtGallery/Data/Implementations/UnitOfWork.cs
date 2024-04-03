using ArtGallery.Data.Abstractions;
using ArtGallery.Models.Structs.Entity;

namespace ArtGallery.Data.Implementations;

public class UnitOfWork(ApplicationDbContext context)
{
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
    private GenericRepository<ApplicationUser>? _userRepository;
    private GenericRepository<Art>? _artRepository;
    private GenericRepository<Artist>? _artistRepository;
    private GenericRepository<Genre>? _genreRepository;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

    public GenericRepository<ApplicationUser> UserRepository => _userRepository ?? new GenericRepository<ApplicationUser>(context);

    public GenericRepository<Art> ArtRepository => _artRepository ?? new GenericRepository<Art>(context);

    public GenericRepository<Artist> ArtistRepository => _artistRepository ?? new GenericRepository<Artist>(context);

    public GenericRepository<Genre> GenresRepository => _genreRepository ?? new GenericRepository<Genre>(context);

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}