using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using movie_list.Models;

namespace movie_list.Areas.Identity.Data;


public class AppUser : IdentityUser
{
    public ICollection<Movie>? WatchList { get; set; }
}

