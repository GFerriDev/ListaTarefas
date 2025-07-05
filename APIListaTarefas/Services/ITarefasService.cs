using AppLista.Server.Models;

namespace AppLista.Server.Services
{
    public interface ITarefaService
    {
        Task<List<Tarefa>> GetAllAsync();
        Task AddAsync(Tarefa tarefa);
        Task<bool> UpdateAsync(int id, Tarefa tarefa);
        Task<bool> ConcluirAsync(int id);
        Task<bool> RemoveAsync(int id);
    }
}
