using Microsoft.EntityFrameworkCore;
using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CommentRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<int> CreateAsync(Comment data)
        {
            applicationDbContext.Comments.Add(data);
            await applicationDbContext.SaveChangesAsync();
            return data.Id;
        }

        public async Task DeleteAsync(int id)
        {
            await applicationDbContext.Comments.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await applicationDbContext.Comments.ToListAsync();
        }

        public async Task<List<Comment>> GetAllByMovieAsync(int movieId)
        {
            return await applicationDbContext.Comments.Where(x => x.MovieId == movieId).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await applicationDbContext.Comments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExistsAsync(int id)
        {
            return await applicationDbContext.Comments.AnyAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Comment data)
        {
            applicationDbContext.Comments.Update(data);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}