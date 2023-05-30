using BeatMatcher.Interfaces;
using BeatMatcher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations.Rules;
using Newtonsoft.Json.Linq;


namespace BeatMatcher.Services
{
    public class SpotifySearchService : ISpotifySearchService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly Secrets _secrets;


        public SpotifySearchService(HttpClient httpClient, IConfiguration configuration, Secrets secrets)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _secrets = secrets;
        }

        public async Task<List<SearchResultDto>> GetSongs(string query, string accessToken)
        {
            string url = $"https://api.spotify.com/v1/search?type=track&q={Uri.EscapeDataString(query)}";

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                JObject data = JObject.Parse(responseBody);

                List<SearchResultDto> results = new List<SearchResultDto>();

                foreach (var item in data["tracks"]["items"])
                {
                    SearchResultDto dto = new SearchResultDto();
                    dto.TrackId = (string)item["id"];
                    dto.TrackName = (string)item["name"];
                    dto.ArtistName = (string)item["artists"][0]["name"];
                    results.Add(dto);
                }

                return results;
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }
        }

        public SearchResultDto SelectSong(List<SearchResultDto> searchResults, int selectedIndex)
        {
            if (selectedIndex >= 0 && selectedIndex < searchResults.Count)
            {
                return searchResults[selectedIndex];
            }

            return null; // or throw an exception if the index is out of range
        }
    }
}
