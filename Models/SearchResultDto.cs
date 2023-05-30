namespace BeatMatcher.Models
{
    public class SearchResultDto
    {
        public string TrackId { get; set; }
        public string TrackName { get; set; }
        public string ArtistName { get; set; }

        public bool IsSelected { get; set; }
    }
}
