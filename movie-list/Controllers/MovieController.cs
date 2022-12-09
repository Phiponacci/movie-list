using Microsoft.AspNetCore.Mvc;
using movie_list.Models;
using System.Diagnostics;
using movie_list.ApiClient;
using movie_list.Data;
using Microsoft.EntityFrameworkCore;
using movie_list.Areas.Identity.Data;

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

        private AppUser? GetUser()
        {
            var username = User.Identity!.Name;
            return (from u in _db.Users
                    where string.Equals(username, u.UserName)
                    select u)
                .Include(_user => _user.WatchList!)
                .FirstOrDefault();
        }

        private List<Movie> GetUserWatchList(AppUser user)
        {
            return (from m in _db.Movies
                    where m.Users!.Count(_user => string.Equals(user.UserName, _user.UserName)) > 0
                    select m).ToList();
        }

        public IActionResult AddToWatchList(string movieName)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Identity/Account/Login");
            }
            var user = GetUser();
            if (user != default(AppUser))
            {
                var watchList = GetUserWatchList(user);
                var movie = (from m in AllMovies
                             where m.name == movieName && watchList.Count(_movie => string.Equals(_movie.name, movieName)) == 0
                             select m).FirstOrDefault();
                if (movie != default(Movie))
                {
                    if ((from m in _db.Movies where string.Equals(m.name, movieName) select m).Count() == 0)
                    {
                        user!.WatchList!.Add(movie);
                        _db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("MoviesList");
        }
    }
}