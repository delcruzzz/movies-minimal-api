﻿using Microsoft.EntityFrameworkCore;
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

        // 1 : M from movies to gendersmovies
        public List<GenderMovie> GenderMovies { get; set; } = new List<GenderMovie>();

        // 1 : M from movies to actorsmovies
        public List<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();
    }
}
