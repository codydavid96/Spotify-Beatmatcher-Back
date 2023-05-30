using BeatMatcher.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using BeatMatcher.Interfaces;

namespace BeatMatcher.Services
{
    public class SpotifyAccountService : ISpotifyAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly Secrets _secrets;

        public SpotifyAccountService(HttpClient httpClient, Secrets secrets)
        {
            _httpClient = httpClient;
            _secrets = secrets;
        }
        public async Task<string> GetToken(string clientId, string clientSecret)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_secrets.ClientId}:{_secrets.ClientSecret}")));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials" }
            });

            var response = await _httpClient.SendAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var authResult = JsonSerializer.Deserialize<AuthResult>(await response.Content.ReadAsStringAsync());

            return authResult.access_token; 
        }
    }
}
