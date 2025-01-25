using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<Categoria>? Categorias { get; set; }
    public DbSet<Produto>? Produtos { get; set; }
    public DbSet<Fornecedor>? Fornecedores { get; set; }
    public DbSet<Usuario>? Usuarios { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>()
            .Property(c => c.Deletado)
            .HasDefaultValue(false);

        modelBuilder.Entity<Produto>()
            .Property(p => p.Deletado)
            .HasDefaultValue(false);

        modelBuilder.Entity<Fornecedor>()
            .Property(f => f.Deletado)
            .HasDefaultValue(false);
        
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Nome)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
