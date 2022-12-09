using movie_list.Models;

namespace movie_list.ApiClient
{
    public class Response
    {
        public Data? data { get; set; }
    }
    public class Data
    {
        public List<Movie>? upcoming { get; set; }
    }
}
