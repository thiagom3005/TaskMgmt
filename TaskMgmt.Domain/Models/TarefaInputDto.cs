using System.ComponentModel.DataAnnotations;
using TaskMgmt.Domain.Enums;

namespace TaskMgmt.Domain.Models
{
    public class TarefaInputDto
    {
        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public StatusTarefa Status { get; set; }

        public DateTime DataVencimento { get; set; }
    }
}
