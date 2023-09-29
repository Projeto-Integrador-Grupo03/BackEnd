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
        }

        public DbSet<Categoria> Categorias { get; set; } = null!;
        
    }
}
