using Microsoft.AspNetCore.Mvc;
using movie_list.Models;
using System.Diagnostics;
using movie_list.ApiClient;

namespace movie_list.Controllers
{
    public class MovieController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly MovieApiClient<Movie> _apiClient;

        public MovieController(ILogger<AuthController> logger, MovieApiClient<Movie> apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public IActionResult MoviesList()
        {
            var response = _apiClient.GetMovies();
            var movies = response.Result.Take(20).ToList();
            return View(movies);
        }

        public IActionResult AddToWatchList()
        {
            return View();
        }
    }
}