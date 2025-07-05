using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AppLista.Server.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [JsonIgnore]
        public List<Tarefa> Tarefas { get; set; } = new();

    }
}
