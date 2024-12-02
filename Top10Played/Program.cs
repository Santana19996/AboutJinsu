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

            // Register SpotifyService
            builder.Services.AddScoped<SpotifyService>(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                var configuration = sp.GetRequiredService<IConfiguration>();

                var clientId = configuration["Spotify:ClientId"];
                var clientSecret = configuration["Spotify:ClientSecret"];
                var refreshToken = configuration["Spotify:RefreshToken"];

                // Validate configuration values
                if (string.IsNullOrWhiteSpace(clientId) || 
                    string.IsNullOrWhiteSpace(clientSecret) || 
                    string.IsNullOrWhiteSpace(refreshToken))
                {
                    throw new InvalidOperationException("Spotify ClientId, ClientSecret, and RefreshToken must be configured in appsettings.json.");
                }

                // Pass values to SpotifyService
                var spotifyService = new SpotifyService(httpClient, clientId, clientSecret);
                spotifyService.SetRefreshToken(refreshToken);

                return spotifyService;
            });

            // Build the app
            var host = builder.Build();

            // Log the secrets (for debugging only)
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
