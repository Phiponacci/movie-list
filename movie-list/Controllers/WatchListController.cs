using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.DataSource.Store;

namespace movie_list.Controllers
{
    public class WatchListController : Controller
    {
        private readonly IStore<Movie> _store;

        public WatchListController(IStore<Movie> store)
        {
            _store = store;
        }

        private AppUser? GetUser()
        {
            var username = User.Identity!.Name;
            return _store.GetUserByUserName(username!);
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
                var watchList = _store.GetWatchListByUserName(user.UserName);
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
            if (user != default(AppUser))
            {
                _store.Delete(user.UserName, movieName);
            }
            return RedirectToAction("MyWatchList");
        }
    }
}