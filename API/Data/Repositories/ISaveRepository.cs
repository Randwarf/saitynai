using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public interface ISaveRepository
    {
        Task<Save?> GetAsync(int id);
        Task<IReadOnlyList<Save>> GetAllAsync();
        Task CreateAsync(Save save);
        Task UpdateAsync(Save save);
        Task DeleteAsync(Save save);
    }
}
