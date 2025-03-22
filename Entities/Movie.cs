using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MoviesMinimalAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;
        public bool onCinemas { get; set; }
        public DateTime ReleaseDate { get; set; }

        [Unicode(false)]
        public string? Poster { get; set; }

        // 1 : M from movies to comments
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
