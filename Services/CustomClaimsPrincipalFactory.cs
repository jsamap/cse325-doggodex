using DoggoDex.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DoggoDex;

// Directly implementing the interface completely bypasses base constructor requirements and the IOptions conflict
public class CustomClaimsPrincipalFactory : IUserClaimsPrincipalFactory<ApplicationUser>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomClaimsPrincipalFactory(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        // Create a clean claims identity instance matching the user
        var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
        
        // Add the mandatory base identity tracking claims required by ASP.NET Core
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
        
        if (!string.IsNullOrEmpty(user.Email))
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        }

        // Inject custom UserType claim directly into the cookie payload
        if (!string.IsNullOrEmpty(user.UserType))
        {
            identity.AddClaim(new Claim("UserType", user.UserType));
        }

        // Return the fully populated principal configuration wrapper
        return new ClaimsPrincipal(identity);
    }
}
