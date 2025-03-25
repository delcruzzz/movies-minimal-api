using Microsoft.EntityFrameworkCore;
using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // create a composite primary key
            modelBuilder.Entity<GenderMovie>().HasKey(g => new { g.GenderId, g.MovieId });
        }

        // add entity sets, i can use for configure the database
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<GenderMovie> GenderMovies { get; set; }
    }
}
