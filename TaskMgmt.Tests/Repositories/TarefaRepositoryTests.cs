using Microsoft.EntityFrameworkCore;
using TaskMgmt.Domain.Entities;
using TaskMgmt.Domain.Enums;
using TaskMgmt.Infrastructure.Data;
using TaskMgmt.Infrastructure.Repositories;

namespace TaskMgmt.Tests.Repositories
{
    public class TarefaRepositoryTests
    {
        private AppDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new AppDbContext(options);
        }

        private TarefaRepository GetRepository(AppDbContext context)
        {
            return new TarefaRepository(context);
        }

        /// <summary>
        /// Testa se o método AddAsync adiciona uma nova tarefa ao banco de dados.
        /// </summary>
        [Fact]
        public async Task AddAsync_ShouldAddTarefa()
        {
            var context = GetDbContext(nameof(AddAsync_ShouldAddTarefa));
            var repo = GetRepository(context);

            var tarefa = new Tarefa
            {
                Titulo = "Teste",
                Descricao = "Descrição",
                Status = StatusTarefa.Pendente,
                DataVencimento = DateTime.Today.AddDays(1)
            };

            await repo.AddAsync(tarefa);

            Assert.Equal(1, context.Tarefas.Count());
            Assert.Equal("Teste", context.Tarefas.First().Titulo);
        }

        /// <summary>
        /// Testa se o método GetByIdAsync retorna a tarefa correta pelo seu Id.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ShouldReturnTarefa()
        {
            var context = GetDbContext(nameof(GetByIdAsync_ShouldReturnTarefa));
            var repo = GetRepository(context);

            var tarefa = new Tarefa
            {
                Titulo = "Teste",
                Descricao = "Descrição",
                Status = StatusTarefa.Pendente,
                DataVencimento = DateTime.Today.AddDays(1)
            };
            context.Tarefas.Add(tarefa);
            context.SaveChanges();

            var result = await repo.GetByIdAsync(tarefa.Id);

            Assert.NotNull(result);
            Assert.Equal("Teste", result!.Titulo);
        }

        /// <summary>
        /// Testa se o método UpdateAsync atualiza corretamente os dados de uma tarefa existente.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ShouldUpdateTarefa()
        {
            var context = GetDbContext(nameof(UpdateAsync_ShouldUpdateTarefa));
            var repo = GetRepository(context);

            var tarefa = new Tarefa
            {
                Titulo = "Teste",
                Descricao = "Descrição",
                Status = StatusTarefa.Pendente,
                DataVencimento = DateTime.Today.AddDays(1)
            };
            context.Tarefas.Add(tarefa);
            context.SaveChanges();

            tarefa.Titulo = "Atualizado";
            await repo.UpdateAsync(tarefa);

            var updated = context.Tarefas.First();
            Assert.Equal("Atualizado", updated.Titulo);
        }

        /// <summary>
        /// Testa se o método DeleteAsync remove uma tarefa existente do banco de dados.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ShouldRemoveTarefa()
        {
            var context = GetDbContext(nameof(DeleteAsync_ShouldRemoveTarefa));
            var repo = GetRepository(context);

            var tarefa = new Tarefa
            {
                Titulo = "Teste",
                Descricao = "Descrição",
                Status = StatusTarefa.Pendente,
                DataVencimento = DateTime.Today.AddDays(1)
            };
            context.Tarefas.Add(tarefa);
            context.SaveChanges();

            await repo.DeleteAsync(tarefa.Id);

            Assert.Empty(context.Tarefas);
        }

        /// <summary>
        /// Testa se o método GetAllAsync retorna tarefas filtradas por status e paginadas corretamente.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_ShouldReturnFilteredAndPaged()
        {
            var context = GetDbContext(nameof(GetAllAsync_ShouldReturnFilteredAndPaged));
            var repo = GetRepository(context);

            for (int i = 1; i <= 20; i++)
            {
                context.Tarefas.Add(new Tarefa
                {
                    Titulo = $"Tarefa {i}",
                    Descricao = "Desc",
                    Status = i % 2 == 0 ? StatusTarefa.Pendente : StatusTarefa.Concluido,
                    DataVencimento = DateTime.Today.AddDays(i)
                });
            }
            context.SaveChanges();

            var result = await repo.GetAllAsync(StatusTarefa.Pendente, null, page: 2, pageSize: 5, sortBy: "titulo", order: "asc");

            Assert.Equal(5, result.Count());
            Assert.All(result, t => Assert.Equal(StatusTarefa.Pendente, t.Status));
        }
    }
}