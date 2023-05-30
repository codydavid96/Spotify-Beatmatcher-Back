using BeatMatcher.Models;

namespace BeatMatcher.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<string>> GetRecommendedTracks(string trackId, string accessToken, int targetBpm, string targetKey, string targetMode);

    }
}
