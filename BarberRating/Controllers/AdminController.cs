using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BarberRating.Services.Interfaces;
using BarberRating.Models.ViewModels;
using BarberRating.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BarberRating.Data.Entities;

namespace BarberRating.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IUserService _userService;
    private readonly IBarberService _barberService;
    private readonly IReviewService _reviewService;

    public AdminController(IWebHostEnvironment hostingEnvironment, IUserService userService, IBarberService barberService, IReviewService reviewService)
    {
        _hostingEnvironment = hostingEnvironment;
        _userService = userService;
        _barberService = barberService;
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var adminViewModel = await GetAdminViewModel();
        return View(adminViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(UserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.RegisterUserAsync(new RegisterViewModel
            {
                Username = model.Username,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName
            });
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        var adminViewModel = await GetAdminViewModel();
        return View("Index", adminViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(UserViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Check if password is provided
            if (string.IsNullOrEmpty(model.Password))
            {
                var existingUser = await _userService.GetUserByIdAsync(model.Id);
                model.Password = existingUser.PasswordHash; 
            }

            var result = await _userService.UpdateUserAsync(model);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        var adminViewModel = await GetAdminViewModel();
        return View("Index", adminViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await _userService.DeleteUserAsync(userId);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Admin");
        }

        var adminViewModel = await GetAdminViewModel();
        return View("Index", adminViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBarber(BarberViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                    // Check if the directory exists, if not, create it
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }

                    // Generate a unique file name to avoid conflicts
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    var imagePath = Path.Combine(imagesPath, uniqueFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                    model.Image = $"/images/{uniqueFileName}"; // Save the URL relative to the web root
                }
                else
                {
                    model.Image = "";
                }

                var barber = new Barber
                {
                    Name = model.Name,
                    Description = model.Description,
                    Image = model.Image
                };

                await _barberService.CreateBarberAsync(barber);
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while uploading the image. Please try again.");
            }
        }

        var adminViewModel = await GetAdminViewModel();
        return View("Index", adminViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBarber(BarberViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var existingBarber = await _barberService.GetBarberByIdAsync(model.Id);
                if (existingBarber == null)
                {
                    return NotFound();
                }

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                    // Check if the directory exists, if not, create it
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }

                    // Generate a unique file name to avoid conflicts
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    var imagePath = Path.Combine(imagesPath, uniqueFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    existingBarber.Image = $"/images/{uniqueFileName}"; // Save the URL relative to the web root
                }
                else if (string.IsNullOrEmpty(existingBarber.Image) && string.IsNullOrEmpty(model.Image))
                {
                    existingBarber.Image = "";
                }

                existingBarber.Name = model.Name;
                existingBarber.Description = model.Description;

                await _barberService.UpdateBarberAsync(existingBarber);
                return RedirectToAction("Index", "Admin");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while uploading the image. Please try again.");
        }

        var adminViewModel = await GetAdminViewModel();
        return View("Index", adminViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBarber(int barberId)
    {
        var result = await _barberService.DeleteBarberAsync(barberId);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Admin");
        }

        var adminViewModel = await GetAdminViewModel();
        return View("Index", adminViewModel);
    }

    private async Task<AdminViewModel> GetAdminViewModel()
    {
        return new AdminViewModel
        {
            Barbers = await _barberService.GetAllBarbersAsync(),
            Users = await _userService.GetAllUsersAsync(),
            UserCount = await _userService.GetUserCountAsync(),
            BarberCount = await _barberService.GetBarberCountAsync(),
            ReviewCount = await _reviewService.GetReviewCountAsync()
        };
    }
}