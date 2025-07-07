using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AppLista.Server.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataConclusao { get; set; }

        public bool FlConcluida { get; set; } = false;

        public int UsuarioId { get; set; }

        public virtual Usuario? Usuario { get; set; }
    }
}
