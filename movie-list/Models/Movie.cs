namespace movie_list.Models
{
    public class PosterImage
    {
        public string? url { get; set; }
    }

    public class Movie
    {
        public string? emsId { get; set; }
        public string? emsVersionId { get; set; }
        public string? name { get; set; }
        public string? releaseDate { get; set; }
        public PosterImage? posterImage { get; set; }
    }
}
