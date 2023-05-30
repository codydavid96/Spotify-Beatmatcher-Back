using BeatMatcher.Interfaces;
using Newtonsoft.Json.Linq;

namespace BeatMatcher.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public RecommendationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<List<string>> GetRecommendedTracks(string trackId, string accessToken, int targetBpm, string targetKey, string targetMode)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            // Create the API request URL with the specified seed track, limit, and additional filters
            var requestUrl = $"https://api.spotify.com/v1/recommendations?seed_tracks={trackId}&limit=10";

            // Add the target BPM filter to the request URL
            requestUrl += $"&target_tempo={targetBpm}";

            // Add the target key filter to the request URL
            requestUrl += $"&target_key={targetKey}";

            // Add the target mode filter to the request URL
            requestUrl += $"&target_mode={targetMode}";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(json);

            var trackIds = new List<string>();

            foreach (var track in jObject["tracks"])
            {
                trackIds.Add(track["id"].ToString());
            }

            return trackIds;
        }
    }
}
