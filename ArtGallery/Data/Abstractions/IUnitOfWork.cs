using ArtGallery.Data.Implementations;
using ArtGallery.Models.Structs.Entity;

namespace ArtGallery.Data.Abstractions;

public interface IUnitOfWork
{
    public IGenericRepository<ApplicationUser> UserRepository { get; }

    public IGenericRepository<Art> ArtRepository { get; }

    public IGenericRepository<Artist> ArtistRepository { get; }

    public IGenericRepository<Genre> GenresRepository { get; }

    public Task SaveAsync();
}