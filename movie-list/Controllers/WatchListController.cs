using Microsoft.AspNetCore.Mvc;
using movie_list.Models;
using movie_list.Data;
using Microsoft.EntityFrameworkCore;
using movie_list.Areas.Identity.Data;

namespace movie_list.Controllers
{
    public class WatchListController : Controller
    {
        private readonly MovieDbContext _db;

        public WatchListController(MovieDbContext db)
        {
            _db = db;
        }

        private AppUser? GetUser()
        {
            var username = User.Identity!.Name;
            return (from u in _db.Users
                    where string.Equals(username, u.UserName)
                    select u)
                .Include(_user => _user.WatchList!)
                .ThenInclude(watchList => watchList.posterImage)
                .FirstOrDefault();
        }

        private List<Movie> GetUserWatchList(AppUser user)
        {
            return (from m in _db.Movies
                   where m.Users!.Count(_user => string.Equals(user.UserName, _user.UserName)) > 0
                   select m).ToList();
        }

        public IActionResult MyWatchList()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Identity/Account/Login");
            }
            var user = GetUser();
            if (user != default(AppUser))
            {
                var watchList = GetUserWatchList(user);
                return View(watchList);
            }
            return View();
        }

        public IActionResult RemoveFromWatchList(string movieName)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Identity/Account/Login");
            }
            var user = GetUser();
            var movie = user!.WatchList!.FirstOrDefault(movie => movie.name!.Equals(movieName));

            if (movie != default(Movie))
            {
                _db.Movies.Remove(movie);
                user!.WatchList!.Remove(movie);
                _db.SaveChanges();
            }
            return RedirectToAction("MyWatchList");
        }
    }
}