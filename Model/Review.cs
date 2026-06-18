namespace DoggoDex.Models;

public class Review
{
    public int Id { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; }

    // Relationship: A review belongs to a specific Dog
    public int DogId { get; set; }
    public Dog Dog { get; set; } = null!;

    // Relationship: A review is written/received by a specific Business Owner
    public int BusinessOwnerId { get; set; }
    public BusinessOwner BusinessOwner { get; set; } = null!;
}
