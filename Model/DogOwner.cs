
namespace DoggoDex.Models;
public class DogOwner
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public byte[]? ProfilePicture { get; set; }
    public ICollection<Dog> Dogs { get; set; } = new List<Dog>();

    public string IdentityUserId { get; set; } = string.Empty;
    public ApplicationUser IdentityUser { get; set; } = null!;
}