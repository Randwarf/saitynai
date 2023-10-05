using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class WorkerRepository : IWorkerRepository
    {
        private GameDbContext _dbContext;

        public WorkerRepository(GameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Worker worker)
        {
            _dbContext.Add(worker);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Worker worker)
        {
            _dbContext.Remove(worker);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Worker>> GetAllAsync(int saveId, int buildingId)
        {
            var workers = await _dbContext.Workers.ToListAsync();
            workers = workers.Where(w => SameBuildingId(w, buildingId) && SameSaveId(w, saveId)).ToList();
            return workers;
        }

        public async Task<Worker?> GetAsync(int saveId, int buildingId, int workerId)
        {
            return await _dbContext.Workers.FirstOrDefaultAsync(w => 
            w.Id == workerId 
            && w.Building.Id == buildingId 
            && w.Building.Save.Id == saveId);
        }

        public async Task UpdateAsync(Worker worker)
        {
            _dbContext.Update(worker);
            await _dbContext.SaveChangesAsync();
        }

        private bool SameBuildingId(Worker worker, int buildingId)
        {
            return worker.Building.Id == buildingId;
        }

        private bool SameSaveId(Worker worker, int saveId)
        {
            return worker.Building.Save.Id == saveId;
        }
    }
}
