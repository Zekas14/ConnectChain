using Microsoft.AspNetCore.Identity;
using ConnectChain.Models;
using ConnectChain.Data.Context;
namespace ConnectChain.Data
{

    public static class DataSeeder
    {
        public static async Task SeedAsync(ConnectChainDbContext context)
        {
            var password = "123456789aA@";
            var hasher = new PasswordHasher<User>();

            if (context.Suppliers.Any())
            {
                var suppliers = new List<Supplier>
            {
                new Supplier
                {
                    Name = "ElectroFix",
                    Address = "Giza",
                    BusinessType = "Electronics",
                    UserName = "electrofix",
                    NormalizedUserName = "ELECTROFIX",
                    Email = "electrofix@example.com",
                    NormalizedEmail = "ELECTROFIX@EXAMPLE.COM",
                    EmailConfirmed = true,
                    ImageUrl = "electrofix.png",
                    PasswordHash = hasher.HashPassword(null, password),
                    Rate = new List<Rate>
                    {
                        new Rate { RateNumber = 5 },
                        new Rate { RateNumber = 4 }
                    }
                },
                new Supplier
                {
                    Name = "CleanIt",
                    Address = "Alexandria",
                    BusinessType = "Cleaning",
                    UserName = "cleanit",
                    NormalizedUserName = "CLEANIT",
                    Email = "cleanit@example.com",
                    NormalizedEmail = "CLEANIT@EXAMPLE.COM",
                    EmailConfirmed = true,
                    ImageUrl = "cleanit.png",
                    PasswordHash = hasher.HashPassword(null, password),
                    Rate = new List<Rate>
                    {
                        new Rate { RateNumber = 3 },
                        new Rate { RateNumber = 5 }
                    }
                }
            };

                await context.Suppliers.AddRangeAsync(suppliers);
            }

            if (context.Set<Customer>().Any())
            {
                var customers = new List<Customer>
            {
                new Customer
                {
                    Name = "Ahmed Zakarya",
                    Address = "Giza",
                    BusinessType = "Electronics",
                    UserName = "ahmedz",
                    NormalizedUserName = "AHMEDZ",
                    Email = "ahmedz@example.com",
                    NormalizedEmail = "AHMEDZ@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, password)
                },
                new Customer
                {
                    Name = "Mona Nabil",
                    Address = "Alexandria",
                    BusinessType = "Cleaning",
                    UserName = "monanabil",
                    NormalizedUserName = "MONANABIL",
                    Email = "monanabil@example.com",
                    NormalizedEmail = "MONANABIL@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, password)
                }
            };

                await context.Set<Customer>().AddRangeAsync(customers);
            }

            await context.SaveChangesAsync();
        }
    }

}
