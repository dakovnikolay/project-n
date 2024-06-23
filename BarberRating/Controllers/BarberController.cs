using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BarberRating.Models;
using BarberRating.Services.Interfaces;
using BarberRating.Data.ViewModels;
using BarberRating.Data.Entities;
using System.Security.Claims;

namespace BarberRating.Controllers;

public class BarberController : Controller
{
    private readonly IBarberService _barberService;
    private readonly IReviewService _reviewService;

    public BarberController(IBarberService barberService, IReviewService reviewService)
    {
        _barberService = barberService;
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var barber = await _barberService.GetBarberByIdAsync(id);
        var reviews = await _reviewService.GetReviewsByBarberIdAsync(id);
        var viewModel = new BarberDetailsViewModel
        {
            Barber = barber,
            Reviews = reviews,
            UserHasReviewed = reviews.Any(r => r.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddReview(Review review)
    {
        review.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims
        await _reviewService.CreateReviewAsync(review);
        return RedirectToAction("Details", new { id = review.BarberId });
    }

    [HttpPost]
    public async Task<IActionResult> EditReview(Review review)
    {
        if (ModelState.IsValid)
        {
            await _reviewService.UpdateReviewAsync(review);
            return RedirectToAction("Details", new { id = review.BarberId });
        }
        // Handle invalid model state, perhaps return to the details view with an error message
        var barber = await _barberService.GetBarberByIdAsync(review.BarberId);
        var reviews = await _reviewService.GetReviewsByBarberIdAsync(review.BarberId);
        var viewModel = new BarberDetailsViewModel
        {
            Barber = barber,
            Reviews = reviews,
            UserHasReviewed = reviews.Any(r => r.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
        };
        return View("Details", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteReview(int reviewId, int barberId)
    {
        await _reviewService.DeleteReviewAsync(reviewId);
        return RedirectToAction("Details", new { id = barberId });
    }

}
