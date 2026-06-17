using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DoggoDex.Models
{
    public class BusinessOwner
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string ContactEmail { get; set; }
        public byte[] ProfilePicture { get; set; }
        public ICollection<Review> Reviews { get; set; }

        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }

    }
}