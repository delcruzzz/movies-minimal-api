using Microsoft.EntityFrameworkCore;
using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        // add entity sets, i can use for configure the database
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
