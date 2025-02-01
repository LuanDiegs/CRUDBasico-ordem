using Infra.Repositorio.Entidades.Personagens.Entidade;
using Microsoft.EntityFrameworkCore;


namespace Infra.Repositorio
{
    public class OrdemDbContext(DbContextOptions<OrdemDbContext> options) : DbContext(options)
    {
        public DbSet<Personagem> Personagens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Personagem>().ToTable("personagem");
        }
    }
}
