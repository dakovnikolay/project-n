using Microsoft.AspNetCore.Identity;

namespace BarberRating.Data.Entities;

public class ApplicationRole : IdentityRole
{
    public string Description { get; set; }
}
