using API.Data.Entities;

namespace API.Data.Repositories
{
    public interface IWorkerRepository
    {
        Task<Worker?> GetAsync(int saveId, int buildingId, int workerId);
        Task<IReadOnlyList<Worker>> GetAllAsync(int saveId, int buildingId);
        Task CreateAsync(Worker worker);
        Task UpdateAsync(Worker worker);
        Task DeleteAsync(Worker worker);
    }
}
