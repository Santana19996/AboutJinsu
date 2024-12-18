using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Top10Played.Models;

namespace Top10Played.Services;

public class SpotifyService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private string _accessToken;
    private string _refreshToken;

    // Constructor accepts configuration values for client ID and secret
    public SpotifyService(HttpClient httpClient, string clientId, string clientSecret)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
        _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
    }

    public void SetRefreshToken(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshToken));
        }

        _refreshToken = refreshToken;
    }

    private async Task RefreshAccessTokenAsync()
    {
        if (string.IsNullOrWhiteSpace(_refreshToken))
        {
            throw new InvalidOperationException("Refresh token has not been set.");
        }

        var requestData = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", _refreshToken },
            { "client_id", _clientId },
            { "client_secret", _clientSecret }
        };

        var response = await _httpClient.PostAsync("https://accounts.spotify.com/api/token", new FormUrlEncodedContent(requestData));

        if (!response.IsSuccessStatusCode)
        {
            var errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to refresh access token: {response.StatusCode} - {errorDetails}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<SpotifyTokenResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _accessToken = tokenResponse.AccessToken;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
    }

    public async Task<List<SpotifyRecentlyPlayedItem>> GetRecentlyPlayedAsync()
    {
        if (string.IsNullOrWhiteSpace(_accessToken))
        {
            await RefreshAccessTokenAsync();
        }

        var response = await _httpClient.GetAsync("https://api.spotify.com/v1/me/player/recently-played?limit=10");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // Refresh token and retry
            await RefreshAccessTokenAsync();
            response = await _httpClient.GetAsync("https://api.spotify.com/v1/me/player/recently-played?limit=10");
        }

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var recentlyPlayed = JsonSerializer.Deserialize<SpotifyRecentlyPlayedResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return recentlyPlayed?.Items ?? new List<SpotifyRecentlyPlayedItem>();
    }
}
