using BarberRating.Data.Entities;

namespace BarberRating.Data.ViewModels;

public class BarberDetailsViewModel
{
    public Barber Barber { get; set; }
    public List<Review> Reviews { get; set; }
    public bool UserHasReviewed { get; set; }
}
