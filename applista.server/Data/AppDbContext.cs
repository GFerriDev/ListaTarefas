using AppLista.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace AppLista.Server.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura FK (opcional pois EF já infere)
            modelBuilder.Entity<Tarefa>()
                .HasOne(t => t.Usuario)
                .WithMany(u => u.Tarefas)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
