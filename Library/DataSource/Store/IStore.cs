using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataSource.Store
{
    public interface IStore<T>
    {
        public IEnumerable<Movie> GetAll();
        public bool MovieExists(string movieName);
        public void Delete(string username, string movieName);
        public AppUser? GetUserByUserName(string userName);
        public List<Movie> GetWatchListByUserName(string userName);
        public void AddMovie(Movie movie);
    }
}
