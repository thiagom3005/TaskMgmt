using Microsoft.EntityFrameworkCore;
using TaskMgmt.Domain.Entities;

namespace TaskMgmt.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Tarefa> Tarefas => Set<Tarefa>();
    }
}