using AppLista.Server.Data;
using AppLista.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace AppLista.Server.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly AppDbContext _context;

        public TarefaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tarefa>> GetAllAsync()
        {
            return await _context.Tarefas
                .Include(t => t.Usuario)
                .OrderBy(t => t.DataCriacao)
                .ToListAsync();
        }

        public async Task AddAsync(Tarefa tarefa)
        {
            tarefa.DataCriacao = DateTime.Now;
            tarefa.FlConcluida = false;

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == tarefa.UsuarioId);
            if (!usuarioExiste)
                throw new Exception("Usuário informado não existe.");

            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, Tarefa atualizada)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return false;

            tarefa.Descricao = atualizada.Descricao;
            tarefa.UsuarioId = atualizada.UsuarioId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return false;

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> ConcluirAsync(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return false;

            tarefa.DataConclusao = DateTime.Now;
            tarefa.FlConcluida = true;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
