#nullable disable

using ArtGallery.Models.Structs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        
        public IList<Role> UserRoles { get; set; }
        
        public class Role()
        {
            public string RoleId { get; set; }
            
            public string RoleName { get; set; }
            
            public bool IsSelected { get; set; }
        }

        public async Task LoadAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            Console.WriteLine(await _userManager.IsInRoleAsync(user, "Admin"));
            if (user == null)
            {
                throw new NullReferenceException();
                //ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                //return View("NotFound");
            }
            //ViewBag.UserName = user.UserName;
            var roles = new List<Role>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new Role()
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

            UserRoles = roles;
        }
        
        public async Task<IActionResult> OnGet(string userId)
        {
            await LoadAsync(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId, IList<Role> userRoles)
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
