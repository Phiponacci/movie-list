using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class PosterImage
    {
        [Key]
        public int Id { get; set; }
        public string? url { get; set; }
    }

    public class Movie
    {
        [Key]
        public string emsId { get; set; }
        public string? name { get; set; }
        public string? releaseDate { get; set; }
        public PosterImage? posterImage { get; set; }
        [Key]
        public AppUser? User { get; set; }
    }
}
