#nullable disable

using ArtGallery.Models.Structs;
using ArtGallery.Models.Structs.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Areas.Identity.Pages.Account.Manage
{
    public class UserRolesManagerModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public UserRolesManagerModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public IEnumerable<UserRoles> Users { get; set; }
        
        public class UserRoles
        {
            public string UserId { get; set; }
            
            public string UserName { get; set; }
            
            public string Email { get; set; }
            
            public IEnumerable<string> Roles { get; set; }
        }
        
        public async Task LoadAsync()
        {
            var users = await  _userManager.Users.ToListAsync();
            var userRoles = new List<UserRoles>();

            foreach (var user in users)
            {
                var userRole = new UserRoles()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await GetUserRoles(user)
                };
                userRoles.Add(userRole);
            }

            Users = userRoles;
        }
        
        public async Task<IActionResult> OnGet()
        {
            await LoadAsync();
            return Page();
        }
        
        private async Task<IEnumerable<string>> GetUserRoles(ApplicationUser user)
        {
            return [..await _userManager.GetRolesAsync(user)];
        }
    }
}
