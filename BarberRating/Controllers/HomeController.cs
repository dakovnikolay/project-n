using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BarberRating.Services.Interfaces;
using BarberRating.Data.ViewModels;

namespace BarberRating.Controllers;

public class HomeController : Controller
{

    private readonly IBarberService _barberService;

    public HomeController(IBarberService barberService)
    {
        _barberService = barberService;
    }


    public async Task<IActionResult> Index()
    {
        var barbers = await _barberService.GetAllBarbersAsync();
        return View(barbers);  
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
