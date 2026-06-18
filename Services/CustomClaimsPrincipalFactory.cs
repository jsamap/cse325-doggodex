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
        // 1. Create a clean claims identity instance matching the user
        var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
        
        // 2. Add the mandatory base identity tracking claims required by ASP.NET Core
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
        
        if (!string.IsNullOrEmpty(user.Email))
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        }

        // 3. Inject your custom UserType claim directly into the cookie payload
        if (!string.IsNullOrEmpty(user.UserType))
        {
            identity.AddClaim(new Claim("UserType", user.UserType));
        }

        // 4. Return the fully populated principal configuration wrapper
        return new ClaimsPrincipal(identity);
    }
}
