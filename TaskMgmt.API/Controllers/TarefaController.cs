using Microsoft.AspNetCore.Mvc;
using TaskMgmt.Domain.Enums;
using TaskMgmt.Domain.Interfaces;
using TaskMgmt.Domain.Mappers;
using TaskMgmt.Domain.Models;

namespace TaskMgmt.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _service;

        public TarefaController(ITarefaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todas as tarefas, com opção de filtro por status e data de vencimento, suporte à paginação e ordenação.
        /// </summary>
        /// <param name="status">Status da tarefa para filtrar (opcional).</param>
        /// <param name="dataVencimento">Data de vencimento para filtrar (opcional).</param>
        /// <param name="page">Número da página para paginação (opcional, padrão = 1).</param>
        /// <param name="pageSize">Quantidade de itens por página (opcional, padrão = 10).</param>
        /// <param name="sortBy">Campo para ordenação (ex: "titulo", "status", "dataVencimento").</param>
        /// <param name="order">Ordem da ordenação: "asc" para crescente ou "desc" para decrescente (padrão = "asc").</param>
        /// <returns>Lista paginada e ordenada de tarefas encontradas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TarefaOutputDto>>> Listar(
            [FromQuery] StatusTarefa? status,
            [FromQuery] DateTime? dataVencimento,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? order = "asc")
        {
            var tarefas = await _service.ListarAsync(status, dataVencimento, page, pageSize, sortBy, order);
            var dtos = tarefas.Select(TarefaMapper.ToOutputDto).ToList();
            foreach (var dto in dtos)
                dto.Links = GerarLinks(dto.Id);
            return Ok(dtos);
        }

        /// <summary>
        /// Obtém uma tarefa pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador da tarefa.</param>
        /// <returns>Tarefa encontrada ou NotFound se não existir.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TarefaOutputDto>> ObterPorId(int id)
        {
            var tarefa = await _service.ObterPorIdAsync(id);
            if (tarefa == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Tarefa não encontrada",
                    Detail = $"Nenhuma tarefa com o id {id} foi encontrada.",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path
                });
            }
            var dto = TarefaMapper.ToOutputDto(tarefa);
            dto.Links = GerarLinks(dto.Id);
            return Ok(dto);
        }

        /// <summary>
        /// Cria uma nova tarefa.
        /// </summary>
        /// <param name="dto">Dados da tarefa a ser criada.</param>
        /// <returns>Tarefa criada.</returns>
        [HttpPost]
        public async Task<ActionResult<TarefaOutputDto>> Criar([FromBody] TarefaInputDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(
                    title: "Dados inválidos",
                    detail: "Um ou mais campos estão inválidos.",
                    statusCode: StatusCodes.Status400BadRequest,
                    instance: HttpContext.Request.Path
                );
            }

            var tarefa = TarefaMapper.ToEntity(dto);
            var criada = await _service.CriarAsync(tarefa);
            var output = TarefaMapper.ToOutputDto(criada);
            output.Links = GerarLinks(output.Id);
            return CreatedAtAction(nameof(ObterPorId), new { id = output.Id }, output);
        }

        /// <summary>
        /// Atualiza uma tarefa existente.
        /// </summary>
        /// <param name="id">Identificador da tarefa a ser atualizada.</param>
        /// <param name="dto">Novos dados da tarefa.</param>
        /// <returns>NoContent se atualizada, NotFound se não existir.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] TarefaInputDto dto)
        {
            var existente = await _service.ObterPorIdAsync(id);
            if (existente == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Tarefa não encontrada",
                    Detail = $"Não foi possível atualizar. Nenhuma tarefa com o id {id} foi encontrada.",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path
                });
            }

            existente.Titulo = dto.Titulo;
            existente.Descricao = dto.Descricao;
            existente.Status = dto.Status;
            existente.DataVencimento = dto.DataVencimento;

            await _service.AtualizarAsync(existente);
            return NoContent();
        }

        /// <summary>
        /// Remove uma tarefa pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador da tarefa a ser removida.</param>
        /// <returns>NoContent se removida, NotFound se não existir.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var existente = await _service.ObterPorIdAsync(id);
            if (existente == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Tarefa não encontrada",
                    Detail = $"Não foi possível remover. Nenhuma tarefa com o id {id} foi encontrada.",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path
                });
            }

            await _service.RemoverAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Gera os links HATEOAS para uma tarefa.
        /// </summary>
        private List<LinkDto> GerarLinks(int id)
        {
            var links = new List<LinkDto>
            {
                new LinkDto
                {
                    Href = Url.Action(nameof(ObterPorId), new { id }) ?? $"api/tarefa/{id}",
                    Rel = "self",
                    Method = "GET"
                },
                new LinkDto
                {
                    Href = Url.Action(nameof(Atualizar), new { id }) ?? $"api/tarefa/{id}",
                    Rel = "update",
                    Method = "PUT"
                },
                new LinkDto
                {
                    Href = Url.Action(nameof(Remover), new { id }) ?? $"api/tarefa/{id}",
                    Rel = "delete",
                    Method = "DELETE"
                }
            };
            return links;
        }
    }
}