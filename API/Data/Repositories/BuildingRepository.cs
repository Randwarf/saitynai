using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private GameDbContext _dbContext;

        public BuildingRepository(GameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Building building)
        {
            _dbContext.Add(building);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Building building)
        {
            _dbContext.Remove(building);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Building>> GetAllAsync(int saveId)
        {
            var buildings = await _dbContext.Buildings.ToListAsync();
            return buildings.Where(building => building.Save.Id == saveId).ToList();
        }

        public async Task<Building?> GetAsync(int saveId, int id)
        {
            return await _dbContext.Buildings.FirstOrDefaultAsync(x => x.Id == id && x.Save.Id == saveId);
        }

        public async Task UpdateAsync(Building building)
        {
            _dbContext.Buildings.Update(building);
            await _dbContext.SaveChangesAsync();
        }
    }
}
