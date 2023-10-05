using ecommerce_iniciativatena.Model;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_iniciativatena.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        { 

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().ToTable("tb_categorias");
            modelBuilder.Entity<Produto>().ToTable("tb_produtos");
            modelBuilder.Entity<Produto>().ToTable("tb_usuarios");

            _ = modelBuilder.Entity<Produto>()
               .HasOne(_ => _.Categoria)
               .WithMany(c => c.Produto)
               .HasForeignKey("CategoriaId")
               .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Produto> Produtos { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        
    }
}
