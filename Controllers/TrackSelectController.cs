using BeatMatcher.Interfaces;
using BeatMatcher.Models;
using BeatMatcher.Services;
using Microsoft.AspNetCore.Mvc;


namespace BeatMatcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrackSelectController
    {
        private readonly ITrackSelectService _trackSelectService;
        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly ISpotifySearchService _spotifySearchService;

        public TrackSelectController(ITrackSelectService trackSelectService, ISpotifyAccountService spotifyAccountService, ISpotifySearchService spotifySearchService)
        {
            _trackSelectService = trackSelectService;
            _spotifyAccountService = spotifyAccountService;
            _spotifySearchService = spotifySearchService;
        }

        [HttpGet]
        // [Route("api/spotify/audio-features")]

        public async Task<TrackSelectDto> Get([FromQuery] string query)
        {
            var clientId = "";
            var clientSecret = "";
            var token = await _spotifyAccountService.GetToken(clientId, clientSecret);

            var searchResults = await _spotifySearchService.GetSongs(query, token);

            if (searchResults.Count > 0)
            {
                var trackId = searchResults[0].TrackId; // Use the first search result
                var track = await _trackSelectService.GetTrack(trackId, token);

                return track;
            }
            else
            {
                throw new InvalidOperationException("No search results found");
            }
        }
    }
}
