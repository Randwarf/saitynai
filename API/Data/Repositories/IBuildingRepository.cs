using API.Data.Entities;

namespace API.Data.Repositories
{
    public interface IBuildingRepository
    {
        Task<Building?> GetAsync(int saveID, int id);
        Task<IReadOnlyList<Building>> GetAllAsync(int saveID);
        Task CreateAsync(Building building);
        Task UpdateAsync(Building building);
        Task DeleteAsync(Building building);
    }
}
