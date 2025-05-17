using TaskMgmt.Domain.Entities;
using TaskMgmt.Domain.Enums;

namespace TaskMgmt.Domain.Interfaces
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> GetAllAsync( StatusTarefa? status, DateTime? dataVencimento, int page = 1, 
            int pageSize = 10, string? sortBy = null, string? order = "asc" );
        Task<Tarefa?> GetByIdAsync(int id);
        Task AddAsync(Tarefa tarefa);
        Task UpdateAsync(Tarefa tarefa);
        Task DeleteAsync(int id);
    }
}