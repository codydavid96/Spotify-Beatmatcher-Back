using BeatMatcher.Interfaces;
using BeatMatcher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using Newtonsoft.Json;

namespace BeatMatcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpotifySearchController : ControllerBase
    {
        private readonly ISpotifySearchService _spotifySearchService;
        private readonly ISpotifyAccountService _spotifyAccountService;
        public SpotifySearchController(ISpotifySearchService spotifySearchService, ISpotifyAccountService spotifyAccountService)
        {
            _spotifySearchService = spotifySearchService;
            _spotifyAccountService = spotifyAccountService;
        }

        [HttpGet]
        [Route("api/spotify/search")]

        public async Task<string> Get(string query)
        {
            var clientId = "";
            var clientSecret = "";
            var token = await _spotifyAccountService.GetToken(clientId, clientSecret);

            var searchResults = await _spotifySearchService.GetSongs(query, token);

            return JsonConvert.SerializeObject(searchResults);
        }

        [HttpPost]
        [Route("api/spotify/select")]
        public SearchResultDto SelectSong(List<SearchResultDto> searchResults, int selectedIndex)
        {
            var selectedSong = _spotifySearchService.SelectSong(searchResults, selectedIndex);
            return selectedSong;
        }
    }
}
