using Microsoft.EntityFrameworkCore;
using System;
using TaskMgmt.Domain.Entities;
using TaskMgmt.Domain.Enums;
using TaskMgmt.Domain.Interfaces;
using TaskMgmt.Infrastructure.Data;

namespace TaskMgmt.Infrastructure.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly AppDbContext _context;

        public TarefaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tarefa>> GetAllAsync(StatusTarefa? status, DateTime? dataVencimento, int page = 1, 
            int pageSize = 10, string? sortBy = null, string? order = "asc")
        {
            var query = _context.Tarefas.AsQueryable();

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);
            if (dataVencimento.HasValue)
                query = query.Where(t => t.DataVencimento.Date == dataVencimento.Value.Date);

            // Ordenação dinâmica
            if (!string.IsNullOrEmpty(sortBy))
            {
                bool asc = order?.ToLower() != "desc";
                query = sortBy.ToLower() switch
                {
                    "titulo" => asc ? query.OrderBy(t => t.Titulo) : query.OrderByDescending(t => t.Titulo),
                    "status" => asc ? query.OrderBy(t => t.Status) : query.OrderByDescending(t => t.Status),
                    "datavencimento" => asc ? query.OrderBy(t => t.DataVencimento) : query.OrderByDescending(t => t.DataVencimento),
                    _ => query.OrderBy(t => t.Id)
                };
            }
            else
            {
                query = query.OrderBy(t => t.Id);
            }

            // Paginação
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<Tarefa?> GetByIdAsync(int id) =>
            await _context.Tarefas.FindAsync(id);

        public async Task AddAsync(Tarefa tarefa)
        {
            await _context.Tarefas.AddAsync(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.Tarefas.FindAsync(id);
            if (task != null)
            {
                _context.Tarefas.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
