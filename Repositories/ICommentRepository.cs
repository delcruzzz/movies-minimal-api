using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI.Repositories
{
    public interface ICommentRepository
    {
        Task<int> CreateAsync(Comment data);
        Task<List<Comment>> GetAllAsync();
        Task<List<Comment>> GetAllByMovieAsync(int movieId);
        Task<Comment?> GetByIdAsync(int id);
        Task<bool> IsExistsAsync(int id);
        Task UpdateAsync(Comment data);
        Task DeleteAsync(int id);
    }
}