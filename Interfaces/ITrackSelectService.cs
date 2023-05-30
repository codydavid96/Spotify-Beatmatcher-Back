using BeatMatcher.Models;

namespace BeatMatcher.Interfaces
{
    public interface ITrackSelectService
    {
        Task<TrackSelectDto> GetTrack(string trackId, string accessToken);
    }
}
