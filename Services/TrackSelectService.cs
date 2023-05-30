using BeatMatcher.Interfaces;
using BeatMatcher.Models;
using Newtonsoft.Json.Linq;

namespace BeatMatcher.Services
{
    public class TrackSelectService : ITrackSelectService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly Secrets _secrets;


        public TrackSelectService(HttpClient httpClient, IConfiguration configuration, Secrets secrets)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _secrets = secrets;
        }

        public async Task<TrackSelectDto> GetTrack(string trackId, string accessToken)
        {
            string url = $"https://api.spotify.com/v1/audio-features/{trackId}";

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                JObject data = JObject.Parse(responseBody);

                TrackSelectDto trackData = new TrackSelectDto();

                trackData.Id = (string)data["id"];
                trackData.Tempo = (string)data["tempo"];
                trackData.Key = (string)data["key"];
                trackData.Mode = (string)data["mode"];

                return trackData;
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }
        }
    }
}
