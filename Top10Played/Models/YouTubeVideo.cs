namespace Top10Played.Models
{
    public class YouTubeVideo
    {
        public string Title { get; set; }
        public string VideoId { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
    }

    // Models for YouTube API response
    public class YouTubeSearchResponse
    {
        public List<YouTubeSearchItem> Items { get; set; }
    }

    public class YouTubeSearchItem
    {
        public YouTubeVideoId Id { get; set; }
        public YouTubeSnippet Snippet { get; set; }
    }

    public class YouTubeVideoId
    {
        public string VideoId { get; set; }
    }

    public class YouTubeSnippet
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public YouTubeThumbnail Thumbnails { get; set; }
    }

    public class YouTubeThumbnail
    {
        public YouTubeThumbnailDetails Medium { get; set; }
    }

    public class YouTubeThumbnailDetails
    {
        public string Url { get; set; }
    }
}