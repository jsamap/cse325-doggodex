namespace DoggoDex.Models;

public class BusinessOwner
{
    public int Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public byte[]? ProfilePicture { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public string IdentityUserId { get; set; } = string.Empty;
    public ApplicationUser IdentityUser { get; set; } = null!;
}