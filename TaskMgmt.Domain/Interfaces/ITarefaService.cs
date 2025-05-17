using TaskMgmt.Domain.Entities;
using TaskMgmt.Domain.Enums;

namespace TaskMgmt.Domain.Interfaces
{
    public interface ITarefaService
    {
        Task<IEnumerable<Tarefa>> ListarAsync(StatusTarefa? status, DateTime? dataVencimento, int page = 1, 
            int pageSize = 10, string? sortBy = null, string? order = "asc");
        Task<Tarefa?> ObterPorIdAsync(int id);
        Task<Tarefa> CriarAsync(Tarefa tarefa);
        Task AtualizarAsync(Tarefa tarefa);
        Task RemoverAsync(int id);
    }
}