using BarberRating.Data.Entities;

namespace BarberRating.Data.ViewModels;

public class AdminViewModel
{
    public List<Barber> Barbers { get; set; }
    public List<ApplicationUser> Users { get; set; }
    public int UserCount { get; set; }
    public int BarberCount { get; set; }
    public int ReviewCount { get; set; }
}
