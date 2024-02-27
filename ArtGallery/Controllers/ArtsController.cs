using ArtGallery.Models;
using ArtGallery.Models.Structs;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers;

public class ArtsController : Controller
{
    public IActionResult Index()
    {
        var arts = GetArts();
        
        return View(arts);
    }

    public IActionResult Artist(string idArt)
    {
        return View();
    }

    public IActionResult Picture()
    {
        return View();
    }

    /*public IActionResult Register()
    {
        return View("~/Areas/Identity/Pages/Account/Register.cshtml");
    }*/

    private IEnumerable<Art> GetArts()
    {
        var arts = new List<Art>();
        
        for (var i = 0; i < 35; i++)
        {
            arts.Add(new Art
            {
                ArtName = "9 вал", 
                ArtPrice = "1000$", 
                Author = "Иван Айвазовский"
            });
        }

        return arts;
    }
}