using System.Collections.Generic;

namespace DoggoDex.Models
{
    public class Dog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public byte[] ProfilePicture { get; set; }
        public double Rating { get; set; }
        public ICollection<Review> Reviews { get; set; }
        
        public int DogOwnerId { get; set; }
        public DogOwner DogOwner { get; set; }
    }
}