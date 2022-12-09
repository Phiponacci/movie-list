using Microsoft.AspNetCore.Mvc;
using movie_list.Models;
using System.Diagnostics;
using movie_list.ApiClient;
using movie_list.Data;
using Microsoft.EntityFrameworkCore;

namespace movie_list.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieApiClient<Movie> _apiClient;
        private readonly MovieDbContext _db;
        private List<Movie> AllMovies;

        public MovieController(MovieApiClient<Movie> apiClient, MovieDbContext db)
        {
            _db = db;
            _apiClient = apiClient;
            var response = _apiClient.GetMovies();
            AllMovies = response.Result.Take(20).ToList();
        }

        public IActionResult MoviesList()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Identity/Account/Login");
            }
            return View(AllMovies);
        }

        public IActionResult AddToWatchList(string movieName)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Identity/Account/Login");
            }
            var username = User.Identity!.Name;
            var user = _db.Users.Include(_user => _user.WatchList).FirstOrDefault(_user => _user.UserName.Equals(username));
            var movie = AllMovies
                .FirstOrDefault(movie => movie.name!.Equals(movieName)
                && user!.WatchList!
                .Count(_movie => _movie.name!.Equals(movieName)) == 0);

            if (movie != default(Movie))
            {
                if (_db.Movies.Count(_movie => _movie.name!.Equals(movieName)) == 0)
                {
                    user!.WatchList!.Add(movie);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("MoviesList");
        }
    }
}