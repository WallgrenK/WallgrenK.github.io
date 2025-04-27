using Server.Models.SeatingModels;
using Server.Models.UserModels;

namespace Server.Models.Interface
{
    public interface ITableService
    {
        Task<IEnumerable<Table>> GetAllAsync();
        Task AddAsync(Table table);
        Task UpdateAsync(Table entityToUpdate, Table entity);
        Task DeleteAsync(Table entity);
        Task<Table?> GetAsync(int id);
    }
}
