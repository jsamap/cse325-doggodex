using Microsoft.AspNetCore.Identity;

namespace DoggoDex.Models;

public class ApplicationUser : IdentityUser
{
    // The user type selected at registration ("DogOwner" or "BusinessOwner")
    public string UserType { get; set; } = string.Empty;

    // Navigation properties for the one-to-one relationships
    public DogOwner? DogOwnerProfile { get; set; }
    public BusinessOwner? BusinessOwnerProfile { get; set; }
}