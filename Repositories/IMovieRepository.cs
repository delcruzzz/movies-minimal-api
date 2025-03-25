using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI.Repositories
{
    public interface IMovieRepository
    {
        Task<int> CreateAsync(Movie data);
        Task DeleteAsync(int id);
        Task<List<Movie>> GetAllAsync(int take, int skip);
        Task<Movie?> GetByIdAsync(int id);
        Task<bool> IsExistsAsync(int id);
        Task UpdateAsync(Movie data);
        Task AssignGendersAsync(int id, List<int> gendersIds);
    }
}
