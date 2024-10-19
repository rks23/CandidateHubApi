using CandidateHubApi.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CandidateHubApi.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove any existing database context registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<CandidateHubContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Create a new service provider
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase() // Use InMemory database for testing
                    .BuildServiceProvider();

                // Add the database context
                services.AddDbContext<CandidateHubContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase"); // Specify your InMemory database name
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // Rebuild the service provider to include the new context
                var serviceProvider2 = services.BuildServiceProvider();
                using (var scope = serviceProvider2.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<CandidateHubContext>();

                    // Ensure the database is created and seed any test data if necessary
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    // Seed test data here if needed
                    SeedTestData(context);
                }
            });
        }

        private void SeedTestData(CandidateHubContext context)
        {
            var candidates = new List<Candidate>();
            for (int i = 1; i <= 20; i++)
            {
                candidates.Add(new Candidate
                {
                    CandidateId = i,
                    FirstName = "FirstName" + i,
                    LastName = "LastName" + i,
                    Email = $"candidate{i}@example.com",
                    GithubURL = string.Empty,
                    LinkedInURL = string.Empty,
                    Bio = $"This is bio for candidate {i}",
                    PhoneNo = $"123-456-789{i}"
                });
            }
            // Add seed data to the context if needed
            context.Candidates.AddRange(candidates);
            context.SaveChanges();
        }
    }
}
