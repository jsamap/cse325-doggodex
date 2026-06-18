using DoggoDex;
using DoggoDex.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

var builder = WebApplication.CreateBuilder(args);

// 1. ADD CORE BLAZOR SERVICES
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

// 2. REGISTER DATABASE CONNECTION (PostgreSQL via Npgsql)
builder.Services.AddDbContext<DoggoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); 

// 3. CONFIGURE IDENTITY SERVICES USING ADDIDENTITYCORE (Bypasses CS0411)
builder.Services.AddIdentityCore<ApplicationUser>(options => 
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false; 
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<DoggoDbContext>()
    .AddSignInManager<SignInManager<ApplicationUser>>() // Explicitly add SignInManager
    .AddDefaultTokenProviders()
    .AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>(); // Injects custom UserType claim

// 4. CONFIGURE COOKIE AUTHENTICATION FOR BLALZOR CIRCUITS
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

// 5. REGISTER BLAZOR SERVER AUTHENTICATION PROVIDER & STATE MANAGERS
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddScoped<UserSessionState>(); 
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// 6. CONFIGURE THE HTTP REQUEST PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 7. ADD SECURITY MIDDLEWARE
app.UseAntiforgery(); 
app.UseAuthentication();
app.UseAuthorization();

// 8. MAP CONTROLLER & BACKEND ROUTING ENDPOINTS
app.MapControllers(); 

// 9. MAP BLAZOR RUNTIME ENDPOINTS
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// 10. INITIALIZE / AUTOMATICALLY SEED DATABASE TABLES
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    try
    {
        await SeedData.InitializeAsync(scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An unhandled exception occurred during background database initialization.");
    }
}

app.Run();
