using Microsoft.EntityFrameworkCore;
using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        // este metodo se usa cuando queremos personalizar el mapeo de EF Core le da a nuestras entidades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // create a composite primary key
            modelBuilder.Entity<GenderMovie>().HasKey(g => new { g.GenderId, g.MovieId });
            modelBuilder.Entity<ActorMovie>().HasKey(m => new { m.ActorId, m.MovieId });
        }

        // add entity sets, i can use for configure the database
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<GenderMovie> GenderMovies { get; set; }
        public DbSet<ActorMovie> ActorsMovies { get; set; }
    }
}
