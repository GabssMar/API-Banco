using ProjetoDsin.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjetoDsin
{
    public class BancoContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<DadosVeiculo> DadosVeiculos { get; set; }
        public DbSet<DadosProprietario> DadosProprietarios { get; set; }
        public DbSet<DetalhesInfracao> DetalhesInfracaos { get; set; }
        public DbSet<Anexos> Anexos { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath = Path.Combine("banco.db");
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }
        
    }
}
