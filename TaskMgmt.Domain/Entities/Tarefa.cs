using TaskMgmt.Domain.Enums;

namespace TaskMgmt.Domain.Entities
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public StatusTarefa Status { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}