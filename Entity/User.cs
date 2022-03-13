using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Medical.Entity;

public class User : IdentityUser
{
    [Required, MaxLength(255)]
    public string Fullname { get; set; }

    [Required]
    public DateTimeOffset Dob { get; set; }

    [MaxLength(1055)]
    public string Bio { get; set; }

    public virtual ICollection<IdentityRole> Roles { get; set; }

    public string? Ratings { get; set; }
}