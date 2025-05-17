using TaskMgmt.Domain.Enums;

namespace TaskMgmt.Domain.Models
{
    public class TarefaOutputDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public StatusTarefa Status { get; set; }
        public DateTime DataVencimento { get; set; }
        public List<LinkDto> Links { get; set; } = new();
    }
}
