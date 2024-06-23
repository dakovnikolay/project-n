namespace BarberRating.Data.Entities;

public class Barber
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }
}
