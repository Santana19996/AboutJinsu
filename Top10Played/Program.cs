using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Top10Played;
using Top10Played.Services;
using System.Net.Http;

namespace Top10Played
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Explicitly add appsettings.json to the configuration
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Register components
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Configure HttpClient for API calls
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Register SpotifyService with the refresh token
            builder.Services.AddScoped<SpotifyService>(sp =>
            {
                var spotifyService = new SpotifyService(sp.GetRequiredService<HttpClient>());

                // Set the Spotify refresh token
                var configuration = sp.GetRequiredService<IConfiguration>();
                var refreshToken = configuration["Spotify:RefreshToken"];
                spotifyService.SetRefreshToken(refreshToken);

                return spotifyService;
            });

            // Build the app
            var host = builder.Build();

            // Log the secrets
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            var config = host.Services.GetRequiredService<IConfiguration>();
            logger.LogInformation("Spotify ClientId: {ClientId}", config["Spotify:ClientId"]);
            logger.LogInformation("Spotify ClientSecret: {ClientSecret}", config["Spotify:ClientSecret"]);
            logger.LogInformation("Spotify RefreshToken: {RefreshToken}");

            // Run the app
            await host.RunAsync();
        }
    }
}