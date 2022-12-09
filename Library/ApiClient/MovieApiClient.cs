using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace movie_list.ApiClient
{
    public class MovieApiClient<T>
    {
        private readonly HttpClient client;
        private readonly string MOVIE_API_URL;
        private readonly string API_KEY;
        public MovieApiClient(IConfiguration configuration)
        {
            MOVIE_API_URL = configuration["MovieApi:Url"];
            API_KEY = configuration["MovieApi:Key"];
            client = new();
        }

        public async Task<List<T>> GetMovies()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(MOVIE_API_URL),
                Headers =
            {
                { "X-RapidAPI-Key", API_KEY },
                { "X-RapidAPI-Host", "flixster.p.rapidapi.com" },
            },
            };
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Response<T>>(body);
            return res?.data?.upcoming!;
        }
    }
}
