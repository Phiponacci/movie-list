
namespace movie_list.ApiClient
{
    public class Response<T>
    {
        public Data<T>? data { get; set; }
    }
    public class Data<T>
    {
        public List<T>? upcoming { get; set; }
    }
}
