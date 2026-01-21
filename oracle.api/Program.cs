/* jenkins pipeline automation testing */
using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using oracle.api.Infrastructure.Contexts;

namespace oracle.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 1. Build the host
            var host = CreateWebHostBuilder(args).Build();

            // 2. Create a scope to get the database context
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<UserDbContext>();
                    
                    // 3. This command triggers the "translator" to create the tables
                    context.Database.Migrate(); 
                }
                catch (Exception ex)
                {
                    // If there is a connection error with Oracle, it will show here
                    Console.WriteLine("An error occurred while migrating the database: " + ex.Message);
                }
            }

            // 4. Run the application
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
