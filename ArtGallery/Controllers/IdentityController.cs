using ArtGallery.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers;

[Area("Identity")]
public class IdentityController : Controller
{
    // GET
    public IActionResult Register()
    {
        return View("Account/Register");
    }
}