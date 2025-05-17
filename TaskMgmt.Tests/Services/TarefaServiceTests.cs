using Moq;
using TaskMgmt.Application.Services;
using TaskMgmt.Domain.Entities;
using TaskMgmt.Domain.Enums;
using TaskMgmt.Domain.Interfaces;

namespace TaskMgmt.Tests.Services
{
    public class TarefaServiceTests
    {
        private readonly Mock<ITarefaRepository> _repoMock;
        private readonly TarefaService _service;

        public TarefaServiceTests()
        {
            _repoMock = new Mock<ITarefaRepository>();
            _service = new TarefaService(_repoMock.Object);
        }

        /// <summary>
        /// Testa se o método ListarAsync retorna a lista de tarefas corretamente,
        /// delegando a busca ao repositório com os parâmetros padrão.
        /// </summary>
        [Fact]
        public async Task ListarAsync_ShouldReturnTarefas()
        {
            // Arrange
            var tarefas = new List<Tarefa>
            {
                new Tarefa { Id = 1, Titulo = "Tarefa 1", Status = StatusTarefa.Pendente, DataVencimento = DateTime.Today }
            };
            _repoMock.Setup(r => r.GetAllAsync(null, null, 1, 10, null, "asc"))
                .ReturnsAsync(tarefas);

            // Act
            var result = await _service.ListarAsync(null, null);

            // Assert
            Assert.Single(result);
            Assert.Equal("Tarefa 1", result.First().Titulo);
        }

        /// <summary>
        /// Testa se o método ObterPorIdAsync retorna a tarefa correta ao buscar pelo Id,
        /// delegando a busca ao repositório.
        /// </summary>
        [Fact]
        public async Task ObterPorIdAsync_ShouldReturnTarefa()
        {
            // Arrange
            var tarefa = new Tarefa { Id = 1, Titulo = "Tarefa 1", Status = StatusTarefa.Pendente, DataVencimento = DateTime.Today };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tarefa);

            // Act
            var result = await _service.ObterPorIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result!.Id);
        }

        /// <summary>
        /// Testa se o método CriarAsync chama o método AddAsync do repositório ao criar uma nova tarefa.
        /// </summary>
        [Fact]
        public async Task CriarAsync_ShouldCallAddAsync()
        {
            // Arrange
            var tarefa = new Tarefa { Id = 1, Titulo = "Nova Tarefa", Status = StatusTarefa.Pendente, DataVencimento = DateTime.Today };

            // Act
            var result = await _service.CriarAsync(tarefa);

            // Assert
            _repoMock.Verify(r => r.AddAsync(tarefa), Times.Once);
            Assert.Equal(tarefa, result);
        }

        /// <summary>
        /// Testa se o método AtualizarAsync chama o método UpdateAsync do repositório ao atualizar uma tarefa existente.
        /// </summary>
        [Fact]
        public async Task AtualizarAsync_ShouldCallUpdateAsync()
        {
            // Arrange
            var tarefa = new Tarefa { Id = 1, Titulo = "Atualizar", Status = StatusTarefa.Pendente, DataVencimento = DateTime.Today };

            // Act
            await _service.AtualizarAsync(tarefa);

            // Assert
            _repoMock.Verify(r => r.UpdateAsync(tarefa), Times.Once);
        }

        /// <summary>
        /// Testa se o método RemoverAsync chama o método DeleteAsync do repositório ao remover uma tarefa pelo Id.
        /// </summary>
        [Fact]
        public async Task RemoverAsync_ShouldCallDeleteAsync()
        {
            // Act
            await _service.RemoverAsync(1);

            // Assert
            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}