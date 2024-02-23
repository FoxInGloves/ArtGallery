using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ArtGallery.Models.Structs;

public class User : IdentityUser
{
    [Key]
    public override string Id { get; set; }

    public override string? UserName { get; set; }

    public override string? Email { get; set; }

    public override string? PasswordHash { get; set; }

    public override bool EmailConfirmed { get; set; }

    public override string? NormalizedUserName { get; set; }

    public override string? NormalizedEmail { get; set; }
}