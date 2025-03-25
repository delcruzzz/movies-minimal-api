using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI.Repositories
{
    public interface IActorRepository
    {
        Task<List<Actor>> GetAllAsync(int take, int skip);
        Task<Actor?> GetByIdAsync(int id);
        Task<int> CreateAsync(Actor data);
        Task UpdateAsync(Actor data);
        Task DeleteAsync(int id);
        Task<bool> IsExistsAsync(int id);
        Task<List<int>> ExistsAsync(List<int> ids);
    }
}
