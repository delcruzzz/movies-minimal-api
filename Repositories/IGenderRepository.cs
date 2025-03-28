using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI.Repositories
{
    public interface IGenderRepository
    {
        Task<int> CreateAsync(Gender data);

        Task<List<Gender>> GetAllAsync();

        Task<Gender?> GetByIdAsync(int id);

        Task<bool> IsExistsAsync(int id);
        Task<List<int>> ExistsAsync(List<int> ids);

        Task UpdateAsync(Gender data);

        Task DeleteAsync(int id);
        Task<bool> SameNameExistsAsync(int id, string name);
    }
}
