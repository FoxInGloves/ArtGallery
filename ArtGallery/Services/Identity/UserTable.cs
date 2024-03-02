using ArtGallery.Models.Structs;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Services.Identity;

public class UserTable<TUser> where TUser : ApplicationUser
{
    private DbContext _context;
    
    public UserTable(DbContext context)
    {
        _context = context;
    }
    
    /*private DbSet<TUser> UsersSet { get { return Context.Set<TUser>(); } }
    private DbSet<TRole> Roles { get { return Context.Set<TRole>(); } }
    private DbSet<TUserClaim> UserClaims { get { return Context.Set<TUserClaim>(); } }
    private DbSet<TUserRole> UserRoles { get { return Context.Set<TUserRole>(); } }
    private DbSet<TUserLogin> UserLogins { get { return Context.Set<TUserLogin>(); } }
    private DbSet<TUserToken> UserTokens { get { return Context.Set<TUserToken>(); } }

    public string GetUserName(string userId)
    {
        _context.
    }*/
}