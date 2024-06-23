using BarberRating.Data;
using BarberRating.Data.Entities;
using BarberRating.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BarberRating.Services;

public class BarberService : IBarberService
{
    private readonly ApplicationDbContext _context;

    public BarberService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Barber>> GetAllBarbersAsync()
    {
        return await _context.Barbers.ToListAsync();
    }

    public async Task<Barber> GetBarberByIdAsync(int barberId)
    {
        return await _context.Barbers.FirstOrDefaultAsync(b => b.Id == barberId);
    }

    public async Task<IdentityResult> CreateBarberAsync(Barber barber)
    {
        try
        {
            _context.Barbers.Add(barber);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError { Description = ex.Message });
        }
    }

    public async Task<IdentityResult> UpdateBarberAsync(Barber barber)
    {
        try
        {
            _context.Barbers.Update(barber);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError { Description = ex.Message });
        }
    }

    public async Task<IdentityResult> DeleteBarberAsync(int barberId)
    {
        var barber = await _context.Barbers.FindAsync(barberId);
        if (barber != null)
        {
            _context.Barbers.Remove(barber);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
        return IdentityResult.Failed(new IdentityError { Description = "Barber not found." });
    }

    public async Task<int> GetBarberCountAsync()
    {
        return await _context.Barbers.CountAsync();
    }
}
