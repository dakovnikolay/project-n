using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BarberRating.Models.ViewModels;

public class BarberViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Barber name is required")]
    [StringLength(64, ErrorMessage = "Barber name must not exceed 64 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(255, ErrorMessage = "Description must not exceed 255 characters")]
    public string Description { get; set; }

    public string? Image { get; set; }

    public IFormFile? ImageFile { get; set; }
}

