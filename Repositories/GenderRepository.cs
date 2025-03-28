﻿using Microsoft.EntityFrameworkCore;
using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI.Repositories
{
    public class GenderRepository : IGenderRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GenderRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int> CreateAsync(Gender data)
        {
            _applicationDbContext.Add(data);
            await _applicationDbContext.SaveChangesAsync();
            return data.Id;
        }

        public async Task DeleteAsync(int id)
        {
            await _applicationDbContext.Genders.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Gender>> GetAllAsync()
        {
            return await _applicationDbContext.Genders.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<Gender?> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Genders.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> IsExistsAsync(int id)
        {
            return await _applicationDbContext.Genders.AnyAsync(g => g.Id == id);
        }

        public async Task<List<int>> ExistsAsync(List<int> ids)
        {
            return await _applicationDbContext.Genders.Where(g => ids.Contains(g.Id)).Select(g => g.Id).ToListAsync();
        }

        public async Task UpdateAsync(Gender data)
        {
            _applicationDbContext.Update(data);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<bool> SameNameExistsAsync(int id, string name)
        {
            return await _applicationDbContext.Genders.AnyAsync(g => g.Id != id && g.Name.ToUpper() == name.ToUpper());
        }
    }
}
