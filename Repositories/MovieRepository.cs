using Microsoft.EntityFrameworkCore;
using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public MovieRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<int> CreateAsync(Movie data)
        {
            applicationDbContext.Movies.Add(data);
            await applicationDbContext.SaveChangesAsync();
            return data.Id;
        }

        public async Task DeleteAsync(int id)
        {
            await applicationDbContext.Movies.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public Task<List<Movie>> GetAllAsync(int take, int skip)
        {
            return applicationDbContext.Movies
                .OrderBy(x => x.Title)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await applicationDbContext.Movies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExistsAsync(int id)
        {
            return await applicationDbContext.Movies.AnyAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Movie data)
        {
            applicationDbContext.Update(data);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
