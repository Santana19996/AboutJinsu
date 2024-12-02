using System.Text.Json.Serialization;

namespace Top10Played.Models
{
    // Token Response Model
    public class SpotifyTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }

    // Recently Played Response Model
    public class SpotifyRecentlyPlayedResponse
    {
        [JsonPropertyName("items")]
        public List<SpotifyRecentlyPlayedItem> Items { get; set; } = new();
    }

    // Recently Played Item Model
    public class SpotifyRecentlyPlayedItem
    {
        [JsonPropertyName("track")]
        public SpotifyTrack Track { get; set; }

        [JsonPropertyName("played_at")]
        public DateTime PlayedAt { get; set; }
    }

    // Track Model
    public class SpotifyTrack
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("artists")]
        public List<SpotifyArtist> Artists { get; set; } = new();

        [JsonPropertyName("album")]
        public SpotifyAlbum Album { get; set; }

        [JsonPropertyName("preview_url")]
        public string PreviewUrl { get; set; }
    }

    // Artist Model
    public class SpotifyArtist
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    // Album Model
    public class SpotifyAlbum
    {
        [JsonPropertyName("images")]
        public List<SpotifyImage> Images { get; set; } = new();
    }

    // Image Model
    public class SpotifyImage
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}