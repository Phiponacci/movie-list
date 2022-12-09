using movie_list.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace movie_list.Models
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
        public string? emsVersionId { get; set; }
        public string? name { get; set; }
        public string? releaseDate { get; set; }
        public PosterImage? posterImage { get; set; }
        public ICollection<AppUser>? Users { get; set; }
    }
}
