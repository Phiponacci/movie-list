using Library.DataSource;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataSource.Store
{

    public class MovieStore : IStore<Movie>
    {
        private readonly MovieDbContext _db;
        public MovieStore(MovieDbContext db)
        {
            _db = db;
        }

        public void AddMovie(Movie movie)
        {
            if (!MovieExists(movie.name!))
            {
                movie.User!.WatchList!.Add(movie);
                _db.SaveChanges();
            }
        }

        public void Delete(string username, string movieName)
        {
            var user = GetUserByUserName(username);
            if (user != default(AppUser))
            {
                var movie = user!.WatchList!.FirstOrDefault(movie => movie.name!.Equals(movieName));

                if (movie != default(Movie))
                {
                    _db.Movies.Remove(movie);
                    user!.WatchList!.Remove(movie);
                    _db.SaveChanges();
                }
            }
        }
        public IEnumerable<Movie> GetAll()
        {
            return _db.Movies;
        }

        public AppUser? GetUserByUserName(string userName)
        {
            return (from u in _db.Users
                    where string.Equals(userName, u.UserName)
                    select u)
                .Include(_user => _user.WatchList!)
                .ThenInclude(watchList => watchList.posterImage)
                .FirstOrDefault();
        }

        public List<Movie> GetWatchListByUserName(string userName)
        {
            return (from m in _db.Movies.Include(movie => movie.User)
                    where string.Equals(userName, m.User!.UserName)
                    select m).ToList();
        }

        public bool MovieExists(string movieName)
        {
            return (from m in _db.Movies where string.Equals(m.name, movieName) select m).Count() != 0;
        }
    }
}
