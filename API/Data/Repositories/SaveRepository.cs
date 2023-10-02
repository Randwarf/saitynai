using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{

    public class SaveRepository : ISaveRepository
    {
        private readonly GameDbContext _dbContext;

        public SaveRepository(GameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Save?> GetAsync(int id)
        {
            return await _dbContext.Saves.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Save>> GetAllAsync()
        {
            return await _dbContext.Saves.ToListAsync();
        }

        public async Task CreateAsync(Save save)
        {
            _dbContext.Saves.Add(save);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Save save)
        {
            _dbContext.Saves.Update(save);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Save save)
        {
            _dbContext.Saves.Remove(save);
            await _dbContext.SaveChangesAsync();
        }
    }
}
