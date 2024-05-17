using ArtGallery.Models.Structs;
using ArtGallery.Models.Structs.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext()
    {
    }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Art> Arts { get; set; }
    
    public DbSet<Artist> Artists { get; set; }
    
    public DbSet<Genre> Genres { get; set; }
}