using Microsoft.AspNetCore.Mvc;
using movie_list.Models;
using System.Diagnostics;
using movie_list.ApiClient;
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

        public IActionResult MyWatchList()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Identity/Account/Login");
            }
            var user = _db.Users.Include(_user => _user.WatchList!).ThenInclude(watchList=>watchList.posterImage).FirstOrDefault(_user => _user.UserName.Equals(User.Identity!.Name));
            if (user != default(AppUser))
            {
                return View(user.WatchList!.ToList());
            }
            return View();
        }

        public IActionResult RemoveFromWatchList(string movieName)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Identity/Account/Login");
            }
            var username = User.Identity!.Name;
            var user = _db.Users.Include(_user => _user.WatchList).FirstOrDefault(_user => _user.UserName.Equals(username));
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