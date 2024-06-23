using BarberRating.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace BarberRating.Services.Interfaces;

public interface IBarberService
{
    Task<List<Barber>> GetAllBarbersAsync();
    Task<Barber> GetBarberByIdAsync(int barId);
    Task<IdentityResult> CreateBarberAsync(Barber barber);
    Task<IdentityResult> UpdateBarberAsync(Barber barber);
    Task<IdentityResult> DeleteBarberAsync(int barberId);
    Task<int> GetBarberCountAsync();
}
