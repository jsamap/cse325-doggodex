using DoggoDex.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DoggoDex;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var context = serviceProvider.GetRequiredService<DoggoDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        Console.WriteLine("Verifying database connection and applying migrations...");
        await context.Database.MigrateAsync();

        // SAFE DATA RESET FOR SUPABASE
        if (context.Users.Any())
        {
            Console.WriteLine("Existing data found. Clearing application tables for fresh seed...");
            await context.Database.ExecuteSqlRawAsync(@"
                TRUNCATE TABLE 
                    ""AspNetUsers"", 
                    ""AspNetRoles"", 
                    ""AspNetUserRoles"", 
                    ""AspNetUserClaims"", 
                    ""AspNetUserLogins"", 
                    ""AspNetUserTokens"", 
                    ""AspNetRoleClaims"", 
                    ""DogOwners"", 
                    ""BusinessOwners"", 
                    ""Dogs"", 
                    ""Reviews"" 
                RESTART IDENTITY CASCADE;");
        }

        // SEED FRESH DATASET
        if (!context.Users.Any())
        {
            Console.WriteLine("Seeding deep relational application data...");

            // SEED BUSINESS OWNERS
            // Business 1
            var bizUser1 = new ApplicationUser
            {
                UserName = "spa@doggodex.com",
                Email = "spa@doggodex.com",
                UserType = "BusinessOwner",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(bizUser1, "Password123!");

            var bizProfile1 = new BusinessOwner
            {
                BusinessName = "Paws & Claws Luxury Spa",
                ContactEmail = "info@pawsandclaws.com",
                IdentityUser = bizUser1
            };
            await context.BusinessOwners.AddAsync(bizProfile1);

            // Business 2 (Elite K9 Training Academy)
            var bizUser2 = new ApplicationUser
            {
                UserName = "academy@doggodex.com",
                Email = "academy@doggodex.com",
                UserType = "BusinessOwner",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(bizUser2, "Password123!");

            var bizProfile2 = new BusinessOwner
            {
                BusinessName = "Elite K9 Training Academy",
                ContactEmail = "trainers@elitek9.com",
                IdentityUser = bizUser2
            };
            await context.BusinessOwners.AddAsync(bizProfile2);


            // SEED DOG OWNERS & THEIR DOGS
            // Owner 1 & Dog 1
            var ownerUser1 = new ApplicationUser
            {
                UserName = "jane@doggodex.com",
                Email = "jane@doggodex.com",
                UserType = "DogOwner",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(ownerUser1, "Password123!");

            var ownerProfile1 = new DogOwner
            {
                FirstName = "Jane",
                LastName = "Doe",
                IdentityUser = ownerUser1
            };
            await context.DogOwners.AddAsync(ownerProfile1);

            var dog1 = new Dog
            {
                Name = "Rocky",
                Breed = "Golden Retriever",
                Rating = 4.5,
                DogOwner = ownerProfile1
            };
            await context.Dogs.AddAsync(dog1);

            // Owner 2 & Dogs 2 and 3
            var ownerUser2 = new ApplicationUser
            {
                UserName = "marcus@doggodex.com",
                Email = "marcus@doggodex.com",
                UserType = "DogOwner",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(ownerUser2, "Password123!");

            var ownerProfile2 = new DogOwner
            {
                FirstName = "Marcus",
                LastName = "Vance",
                IdentityUser = ownerUser2
            };
            await context.DogOwners.AddAsync(ownerProfile2);

            var dog2 = new Dog
            {
                Name = "Bella",
                Breed = "French Bulldog",
                Rating = 5.0,
                DogOwner = ownerProfile2
            };
            var dog3 = new Dog
            {
                Name = "Zeus",
                Breed = "German Shepherd",
                Rating = 4.0,
                DogOwner = ownerProfile2
            };
            await context.Dogs.AddRangeAsync(dog2, dog3);

            // Owner 3 & Dog 4 (Max the Husky)
            var ownerUser3 = new ApplicationUser
            {
                UserName = "sarah@doggodex.com",
                Email = "sarah@doggodex.com",
                UserType = "DogOwner",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(ownerUser3, "Password123!");

            var ownerProfile3 = new DogOwner
            {
                FirstName = "Sarah",
                LastName = "Smith",
                IdentityUser = ownerUser3
            };
            await context.DogOwners.AddAsync(ownerProfile3);

            // Fetch the image from your request via an HTTP stream or localfile
            byte[]? maxImageBytes = null;
            try
            {
                var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                string filePath = Path.Combine(env.WebRootPath, "img", "dogs", "max.jpg");
                maxImageBytes = File.ReadAllBytes(filePath);

                // Using HTTP client
                // using var httpClient = new HttpClient();
                // maxImageBytes = await httpClient.GetByteArrayAsync("img/dogs/max.jpg");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Seed Warning]: Could not fetch Max's profile image download: {ex.Message}");
            }

            var dog4 = new Dog
            {
                Name = "Max",
                Breed = "Siberian Husky",
                Rating = 5.0,
                ProfilePicture = maxImageBytes,
                DogOwner = ownerProfile1
            };
            await context.Dogs.AddAsync(dog4);

            // Add Sky
            // Fetch the image from your request via an HTTP stream or localfile
            byte[]? skyImageBytes = null;
            try
            {
                var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                string filePath = Path.Combine(env.WebRootPath, "img", "dogs", "sky.webp");
                skyImageBytes = File.ReadAllBytes(filePath);

                // Using HTTP client
                // using var httpClient = new HttpClient();
                // maxImageBytes = await httpClient.GetByteArrayAsync("img/dogs/max.jpg");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Seed Warning]: Could not fetch Sky's profile image download: {ex.Message}");
            }

            var dog5 = new Dog
            {
                Name = "Sky",
                Breed = "Siberian Husky",
                Rating = 5.0,
                ProfilePicture = skyImageBytes,
                DogOwner = ownerProfile1
            };
            await context.Dogs.AddAsync(dog5);
            


            // SEED INTERRELATED REVIEWS

            var reviews = new List<Review>
            {
                new() {
                    Comment = "Rocky was an absolute joy to groom! So well behaved during his bubble bath.",
                    Rating = 5,
                    Dog = dog1,
                    BusinessOwner = bizProfile1
                },
                new() {
                    Comment = "Rocky did great on basic commands, but gets distracted by tennis balls easily.",
                    Rating = 4,
                    Dog = dog1,
                    BusinessOwner = bizProfile2
                },
                new() {
                    Comment = "Bella is the sweetest little Frenchie! Perfect posture during the nail trimming.",
                    Rating = 5,
                    Dog = dog2,
                    BusinessOwner = bizProfile1
                },
                new() {
                    Comment = "Zeus showed incredible focus during protective agility training. Excellent instincts.",
                    Rating = 4,
                    Dog = dog3,
                    BusinessOwner = bizProfile2
                },
                new() {
                    Comment = "Max showed outstanding athletic skill on the agility platform today! His focused posture and confidence on the elevated planks are excellent.",
                    Rating = 5,
                    Dog = dog4,
                    BusinessOwner = bizProfile2
                },
                new() {
                    Comment = "Max showed incredible focus during protective agility training. Excellent instincts.",
                    Rating = 5,
                    Dog = dog4,
                    BusinessOwner = bizProfile1
                }
            };

            await context.Reviews.AddRangeAsync(reviews);

            // Save all structural changes to PostgreSQL using the DataContext
            await context.SaveChangesAsync();
            Console.WriteLine("Deep seeding of multiple objects completed successfully.");
        }
    }
}
