using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MoviesMinimalAPI.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public DateTime Birthdate { get; set; }

        [Unicode(false)]
        public string? Photo { get; set; }

        // 1 : M from actor to actormovie
        public List<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();
    }
}
