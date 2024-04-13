#nullable disable

using ArtGallery.Models.Structs;
using ArtGallery.Models.Structs.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Areas.Identity.Pages.Account.Manage
{
    public class ManageUserRolesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUserRolesModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public IList<RoleModel> Roles { get; set; }
        
        public class RoleModel
        {
            public string RoleId { get; set; }
            
            public string RoleName { get; set; }
            
            public bool IsSelected { get; set; }
        }

        public async Task LoadAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null)
            {
                throw new NullReferenceException();
                //ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                //return View("NotFound");
            }
            //ViewBag.UserName = user.UserName;
            var roles = new List<RoleModel>();
            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                var userRolesViewModel = new RoleModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                roles.Add(userRolesViewModel);
            }

            Roles = roles;
        }
        
        public async Task<IActionResult> OnGet(string userId)
        {
            await LoadAsync(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId, IList<RoleModel> userRoles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NullReferenceException();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                //return View(model);
            }
            
            result = await _userManager.AddToRolesAsync(user, userRoles.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                //return View(model);
            }
            return RedirectToPage("./UserRolesManager");
        }
    }
}
