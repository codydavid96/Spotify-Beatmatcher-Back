using BeatMatcher.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeatMatcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpotifyAccountController : ControllerBase
    {
        private readonly ISpotifyAccountService _spotifyAccountService;

        public SpotifyAccountController(ISpotifyAccountService spotifyAccountService)
        {
            _spotifyAccountService = spotifyAccountService;
        }

        [HttpGet]

        public async Task<string> Get()
        {
            var token = await _spotifyAccountService.GetToken("", "");

            return token;
        }
    
    }

}