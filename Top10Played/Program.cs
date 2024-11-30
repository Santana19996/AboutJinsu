using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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
                spotifyService.SetRefreshToken("AQBWYI3o1rVdn9eWdMZb9vydSQA00BKHUHUvmdTk6aoe7Yn-hzyCMCu2NTMyXXxiYNuEyf3IXKHvxTvpGq--PDokS_B64kptJTVvXq9BSpJplYn9cAfvESRQ7-i09Fj0QwA");

                return spotifyService;
            });

            // Register YouTubeService with the refresh token
            // builder.Services.AddScoped<YouTubeService>(sp =>
            
              //  var youtubeService = new YouTubeService(sp.GetRequiredService<HttpClient>());
                
                // Set the YouTube refresh token
                // youtubeService.SetRefreshToken("1//03jtXrtZ-jPt8CgYIARAAGAMSNwF-L9IrInrdzfMBpnm7kRLOY4ss_X5afFwux-IqZmeLuVi-hfTOWry0Nsl2-tszPqgDz7ACTUQ");
                //
                // return youtubeService;
            

            // Build and run the app
            await builder.Build().RunAsync();
        }
    }
}