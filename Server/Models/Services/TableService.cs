using Server.Models.Interface;
using Server.Models.SeatingModels;

namespace Server.Models.Services
{
    public class TableService : ITableService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<TableService> _logger;

        public TableService(ILogger<TableService> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task AddAsync(Table table)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Table entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Table>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Table?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Table entityToUpdate, Table entity)
        {
            throw new NotImplementedException();
        }
    }
}
