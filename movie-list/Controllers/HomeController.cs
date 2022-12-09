using Microsoft.AspNetCore.Mvc;
using movie_list.Models;
using System.Diagnostics;
using movie_list.ApiClient;

namespace movie_list.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}