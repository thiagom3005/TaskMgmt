using TaskMgmt.Domain.Entities;
using TaskMgmt.Domain.Models;

namespace TaskMgmt.Domain.Mappers
{
    public static class TarefaMapper
    {
        public static TarefaOutputDto ToOutputDto(Tarefa tarefa) => new()
        {
            Id = tarefa.Id,
            Titulo = tarefa.Titulo,
            Descricao = tarefa.Descricao,
            Status = tarefa.Status,
            DataVencimento = tarefa.DataVencimento
        };

        public static Tarefa ToEntity(TarefaInputDto dto) => new()
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Status = dto.Status,
            DataVencimento = dto.DataVencimento
        };
    }
}