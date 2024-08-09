using Microsoft.EntityFrameworkCore;
using MyAPI.Models;

namespace MyAPI.Context
{
    /// <summary>
    /// Classe de contexto que vai herdar (DbContext) Entity Framework Core.
    /// Nessa classe nós como desenvolvedores vamos definir o mapeamento entre as entidades e as tabelas do banco de dados.
    ///     entidades - representação das tabelas do banco na nossa aplicação
    ///     DbContext - Representa uma sessão com o banco de dados sendo a "ponte" entre as atividades de domínio e o banco
    /// </summary>
    public class AppDbContext : DbContext
    {
        //options - serão as configurações do contexto, string de conexão com o banco de dados
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //dbSet representa a coleção de entidades no contexto que podem ser consultadas - TABELAS
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        // ao aplicar override o método original será reescrito pelo novo método 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
