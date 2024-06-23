using BarberRating.Data.Entities;
using BarberRating.Data.ViewModels;
using BarberRating.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BarberRating.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);
        Task<SignInResult> LoginUserAsync(LoginViewModel model);
        Task LogoutUserAsync();
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<IdentityResult> UpdateUserAsync(UserViewModel user);
        Task<IdentityResult> DeleteUserAsync(string userId);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<int> GetUserCountAsync();
    }

}