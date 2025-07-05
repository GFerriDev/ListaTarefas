using AppLista.Server.Models;

namespace AppLista.Server.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario> AddAsync(Usuario usuario);
        Task<bool> UpdateAsync(int id, Usuario usuario);
        Task<bool> DeleteAsync(int id);
    }
}
