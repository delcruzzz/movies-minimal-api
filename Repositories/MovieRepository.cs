using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        public MovieRepository(ApplicationDbContext applicationDbContext , IMapper mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
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
            return await applicationDbContext.Movies
                .Include(p => p.Comments)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task AssignGendersAsync(int id, List<int> gendersIds)
        {
            var movie = await applicationDbContext.Movies
                .Include(p => p.GenderMovies).FirstOrDefaultAsync(p => p.Id == id);

            if (movie is null)
            {
                throw new ArgumentException($"Movie not found");
            }

            var gendersMovies = gendersIds.Select(genderId => new GenderMovie() { GenderId = genderId });
            movie.GenderMovies = mapper.Map(gendersMovies, movie.GenderMovies);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
