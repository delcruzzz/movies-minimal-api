using Microsoft.EntityFrameworkCore;
using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ActorRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<int> CreateAsync(Actor data)
        {
            applicationDbContext.Actors.Add(data);
            await applicationDbContext.SaveChangesAsync();
            return data.Id;
        }

        public async Task DeleteAsync(int id)
        {
            await applicationDbContext.Actors.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Actor>> GetAllAsync(int take, int skip)
        {
            return await applicationDbContext.Actors
                .OrderBy(x => x.Name)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        public async Task<Actor?> GetByIdAsync(int id)
        {
            return await applicationDbContext.Actors.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExistsAsync(int id)
        {
            return await applicationDbContext.Actors.AnyAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Actor data)
        {
            applicationDbContext.Update(data);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<List<int>> ExistsAsync(List<int> ids)
        {
            return await applicationDbContext.Actors.Where(a => ids.Contains(a.Id)).Select(a => a.Id).ToListAsync();
        }
    }
}
