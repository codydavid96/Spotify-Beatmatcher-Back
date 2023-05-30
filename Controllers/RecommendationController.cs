using BeatMatcher.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeatMatcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;
        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly ITrackSelectService _trackSelectService;

        public RecommendationController(IRecommendationService recommendationService, ISpotifyAccountService spotifyAccountService, ITrackSelectService trackSelectService)
        {
            _recommendationService = recommendationService;
            _spotifyAccountService = spotifyAccountService;
            _trackSelectService = trackSelectService;
        }

        [HttpGet("{trackId}")]
        public async Task<ActionResult<List<string>>> GetRecommendedTracks(string trackId, int targetBpm, string targetKey, string targetMode)
        {
            var clientId = "";
            var clientSecret = "";
            var token = await _spotifyAccountService.GetToken(clientId, clientSecret);

            // Call the search service to get the selected track
            var searchResults = await _spotifySearchService.GetSongs("query", token);

            // Select the track from the search results
            var selectedTrack = _spotifySearchService.SelectSong(searchResults, Convert.ToInt32(trackId));

            if (selectedTrack == null)
            {
                return BadRequest("Invalid track selected.");
            }

            // Extract the trackId from the selected track
            var selectedTrackId = selectedTrack.TrackId;

            var recommendedTracks = await _recommendationService.GetRecommendedTracks(selectedTrackId, token, targetBpm, targetKey, targetMode);
            return recommendedTracks;
        }
        
    }
}
