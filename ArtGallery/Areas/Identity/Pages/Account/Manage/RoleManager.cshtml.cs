using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Areas.Identity.Pages.Account.Manage
{
    public class RoleManagerModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleManagerModel> _logger;

        public RoleManagerModel(
            RoleManager<IdentityRole> roleManager, 
            ILogger<RoleManagerModel> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
            Console.WriteLine("Создание класса");
        }
        
        [TempData]
        public string StatusMessage { get; set; }
        
        public IList<IdentityRole> Roles { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
            
        public class InputModel
        {
            [Required]
            [StringLength(30, MinimumLength = 1, ErrorMessage = "The length of {0} must be at least {2} and no more than {1} characters.")]
            [Display(Name = "New role")]
            public string? NewRole { get; set; }
        }
        
        private async Task LoadAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            Roles = roles;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            await LoadAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var newRole = Input.NewRole?.Trim();

            if (string.IsNullOrEmpty(newRole))
            {
                StatusMessage = "Error, a role cannot be added";
                return RedirectToPage();
            }
            
            var roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            if (roles.Contains(newRole))
            {
                //TODO Сменить Error на Ошибка
                StatusMessage = "Error, such a role already exists";
                return Page();
            }
            
            await _roleManager.CreateAsync(new IdentityRole(newRole));
            StatusMessage = $"Role '{newRole}' added";
            _logger.LogInformation("New role added successfully");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string roleId)
        {
            //TODO как нибудь переписать LoadAsync
            //await LoadAsync();
            
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                StatusMessage = "Error that role doesn't exist!";
                return Page();
            }

            await _roleManager.DeleteAsync(role);
            StatusMessage = $"Role '{role}' successfully deleted.";
            _logger.LogInformation("RoleModel deleted");
            return RedirectToPage();
        }
        
        /*private List<IdentityRole> GetRoles()
        {
            return _roleManager.Roles.ToList();
        }*/
    }
}
