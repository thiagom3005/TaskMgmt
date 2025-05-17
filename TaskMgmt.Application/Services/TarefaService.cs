using TaskMgmt.Domain.Entities;
using TaskMgmt.Domain.Enums;
using TaskMgmt.Domain.Interfaces;

namespace TaskMgmt.Application.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _repository;

        public TarefaService(ITarefaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Tarefa>> ListarAsync(StatusTarefa? status, DateTime? dataVencimento, int page = 1, 
            int pageSize = 10, string? sortBy = null, string? order = "asc")
        {
            return await _repository.GetAllAsync(status, dataVencimento, page, pageSize, sortBy, order);
        }

        public async Task<Tarefa?> ObterPorIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<Tarefa> CriarAsync(Tarefa tarefa)
        {
            await _repository.AddAsync(tarefa);
            return tarefa;
        }

        public async Task AtualizarAsync(Tarefa tarefa)
            => await _repository.UpdateAsync(tarefa);

        public async Task RemoverAsync(int id)
            => await _repository.DeleteAsync(id);
    }
}