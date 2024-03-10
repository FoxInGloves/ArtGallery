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
        }
        
        [TempData]
        public string StatusMessage { get; set; }
        
        //[BindProperty]
        public IList<IdentityRole> Roles { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
            
        public class InputModel
        {
            [Required]
            [StringLength(30, MinimumLength = 1, ErrorMessage = "Длина {0} должна быть не менее {2} и не более {1} символов.")]
            [Display(Name = "Новая роль")]
            public string NewRole { get; set; }
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
            var newRole = Input.NewRole.Trim();
            
            var roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            if (roles.Contains(newRole))
            {
                //TODO Сменить Error на Ошибка
                StatusMessage = "Error Такая роль уже существует";
                return Page();
            }
            
            await _roleManager.CreateAsync(new IdentityRole(newRole));
            StatusMessage = $"Роль '{newRole}' добавлена";
            _logger.LogInformation("New role added successfully");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string roleId)
        {
            //TODO как нибудь переписать LoadAsync
            await LoadAsync();
            
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return NotFound($"Couldn't find role {roleId}");
            }

            if (!Roles.Contains(role))
            {
                StatusMessage = "Error такой роли не существует!";
                return Page();
            }

            await _roleManager.DeleteAsync(role);
            StatusMessage = "Роль успешно удалена.";
            _logger.LogInformation("Role deleted");
            return RedirectToPage();
        }
        
        /*private List<IdentityRole> GetRoles()
        {
            return _roleManager.Roles.ToList();
        }*/
    }
}
