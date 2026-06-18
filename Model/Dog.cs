namespace DoggoDex.Models;

public class Dog
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public byte[]? ProfilePicture { get; set; } // Nullable for seeding
    public double Rating { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    
    // Foreign Key matching DogOwner table
    public int DogOwnerId { get; set; }
    public DogOwner DogOwner { get; set; } = null!;
}
