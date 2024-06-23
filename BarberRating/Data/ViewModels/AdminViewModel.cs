using BarberRating.Data.Entities;

namespace BarberRating.Data.ViewModels;

public class AdminViewModel
{
    public List<Barber> Bars { get; set; }
    public List<ApplicationUser> Users { get; set; }
    public int UserCount { get; set; }
    public int BarCount { get; set; }
    public int ReviewCount { get; set; }
}
