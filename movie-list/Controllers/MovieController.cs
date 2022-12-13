using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using movie_list.ApiClient;
using Microsoft.EntityFrameworkCore;
using Library.Models;
using Library.DataSource.Store;

namespace movie_list.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieApiClient<Movie> _apiClient;
        private readonly IStore<Movie> _store;
        private List<Movie> AllMovies;

        private bool isFailed = false;

        public MovieController(MovieApiClient<Movie> apiClient, IStore<Movie> store)
        {
            _apiClient = apiClient;
            _store = store;
            try
            {
                var response = _apiClient.GetMovies();
                AllMovies = response.Result.Take(20).ToList();
            }
            catch (Exception)
            {
                isFailed = true;
            }
        }

        public IActionResult MoviesList()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Identity/Account/Login");
            }
            TempData["Error"] = isFailed;
            if (isFailed)
                return View();
            return View(AllMovies);
        }

        private AppUser? GetUser()
        {
            var username = User.Identity!.Name;
            return _store.GetUserByUserName(username!);
        }


        public IActionResult AddToWatchList(string movieName)
        {
            TempData["Error"] = isFailed;
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Identity/Account/Login");
            }
            var user = GetUser();
            if (user != default(AppUser))
            {
                var watchList = _store.GetWatchListByUserName(user.UserName);
                var movie = (from m in AllMovies
                             where m.name == movieName && watchList.Count(_movie => string.Equals(_movie.name, movieName)) == 0
                             select m).FirstOrDefault();
                if (movie != default(Movie))
                {
                    movie.User = user;
                    _store.AddMovie(movie);
                }
            }
            return RedirectToAction("MoviesList");
        }
    }
}