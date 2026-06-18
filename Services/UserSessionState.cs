using System.Security.Claims;
using DoggoDex.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DoggoDex;

public class UserSessionState
{
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly IServiceProvider _serviceProvider;

    public string? DisplayName { get; private set; }
    public bool IsLoaded { get; private set; }

    public UserSessionState(AuthenticationStateProvider authStateProvider, IServiceProvider serviceProvider)
    {
        _authStateProvider = authStateProvider;
        _serviceProvider = serviceProvider;
    }

    public async Task EnsureProfileLoadedAsync()
    {
        // If already populated, exit instantly
        if (IsLoaded && !string.IsNullOrEmpty(DisplayName)) return;

        try
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is { IsAuthenticated: true })
            {
                var userType = user.FindFirst("UserType")?.Value;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<DoggoDbContext>();

                    if (userType == "DogOwner")
                    {
                        var ownerProfile = await dbContext.DogOwners
                            .AsNoTracking()
                            .FirstOrDefaultAsync(o => o.IdentityUserId == userId);
                        DisplayName = ownerProfile?.FirstName;
                    }
                    else if (userType == "BusinessOwner")
                    {
                        var businessProfile = await dbContext.BusinessOwners
                            .AsNoTracking()
                            .FirstOrDefaultAsync(b => b.IdentityUserId == userId);
                        DisplayName = businessProfile?.BusinessName;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Catch silently: Indicates we are likely in static prerender mode.
            // The method will execute again cleanly during interactive connection setup.
            Console.WriteLine($"[Session Warning during init]: {ex.Message}");
        }

        IsLoaded = true;
    }
}
