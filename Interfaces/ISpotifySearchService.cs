using BeatMatcher.Models;

namespace BeatMatcher.Interfaces
{
    public interface ISpotifySearchService
    {
        Task<List<SearchResultDto>> GetSongs(string query, string accessToken);
        SearchResultDto SelectSong(List<SearchResultDto> searchResults, int selectedIndex);
    }
}
