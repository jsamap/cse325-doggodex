using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DoggoDex.Models
{
    public class DogOwner
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] ProfilePicture { get; set; }
        public ICollection<Dog> Dogs { get; set; }

        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}