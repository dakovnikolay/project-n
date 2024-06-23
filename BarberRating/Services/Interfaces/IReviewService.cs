using BarberRating.Data.Entities;

namespace BarberRating.Services.Interfaces;

public interface IReviewService
{
    Task<List<Review>> GetReviewsByBarberIdAsync(int barberId);
    Task<Review> GetReviewByIdAsync(int reviewId);
    Task<Review> GetUserReviewByBarberIdAsync(string userId, int barberId);
    Task CreateReviewAsync(Review review);
    Task UpdateReviewAsync(Review review);
    Task DeleteReviewAsync(int reviewId);
    Task<int> GetReviewCountAsync();
}