using DoggoDex.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoggoDex.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly DoggoDbContext _context;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        DoggoDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpPost("register")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> RegisterFromForm([FromForm] IFormCollection form)
    {
        try
        {
            var email = form["Email"].ToString();
            var password = form["Password"].ToString();
            var userType = form["UserType"].ToString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return Content("Validation Error: Email and password fields cannot be blank.");
            }

            // Use application user to bind additional properties to the Framework user
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                UserType = userType,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var description = string.Join(" | ", result.Errors.Select(e => e.Description));
                Console.WriteLine($"[Identity Error]: {description}");
                return Content($"Registration validation failed: {description}");
            }

            // User type could be DogOwner or BusinessOwner
            if (userType == "DogOwner")
            {
                var dogOwner = new DogOwner
                {
                    FirstName = form["FirstName"].ToString(),
                    LastName = form["LastName"].ToString(),
                    IdentityUser = user
                };
                await _context.DogOwners.AddAsync(dogOwner);
            }
            else if (userType == "BusinessOwner")
            {
                var businessOwner = new BusinessOwner
                {
                    BusinessName = form["BusinessName"].ToString(),
                    ContactEmail = form["ContactEmail"].ToString(),
                    IdentityUser = user
                };
                await _context.BusinessOwners.AddAsync(businessOwner);
            }

            await _context.SaveChangesAsync();
            await _signInManager.SignInAsync(user, isPersistent: true);
            return Redirect("/");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Critical Backend Error]: {ex.Message}");
            return Content($"Internal operational failure: {ex.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginFromForm([FromForm] IFormCollection form)
    {
        var email = form["Email"].ToString();
        var password = form["Password"].ToString();
        var rememberMe = form["RememberMe"].ToString() == "true";

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return Redirect("/login?error=Email and password cannot be empty.");
        }

        // Attempt to sign the user in
        var result = await _signInManager.PasswordSignInAsync(
            email,
            password,
            isPersistent: rememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return Redirect("/");
        }

        // Handle generic failure
        return Redirect("/login?error=Invalid email or password layout.");
    }

    [HttpGet("logout")]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/login");
    }


}
