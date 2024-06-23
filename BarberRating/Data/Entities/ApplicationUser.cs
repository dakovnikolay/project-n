using Microsoft.AspNetCore.Identity;

namespace BarberRating.Data.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }
}
