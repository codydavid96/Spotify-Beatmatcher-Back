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
        [Route("api/spotify/audio-features")]

        public async Task<TrackSelectDto> Get(string trackId, int selectedIndex)
        {
            var clientId = "";
            var clientSecret = "";
            var token = await _spotifyAccountService.GetToken(clientId, clientSecret);

            var searchResults = await _spotifySearchService.GetSongs(trackId, token);

            if (searchResults.Count > 0 && selectedIndex >= 0 && selectedIndex < searchResults.Count)
            {
                var selectedTrack = searchResults[selectedIndex];
                var track = await _trackSelectService.GetTrack(selectedTrack.TrackId, token);

                return track;
            }
            else
            {
                throw new InvalidOperationException("Invalid search result index or no search results found");
            }
        }

    }
}
