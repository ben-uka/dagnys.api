using Dagnys_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dagnys_API.Data;

public class DataContext : DbContext
{
    public DbSet<ProductsModel> Products => Set<ProductsModel>();
    public DbSet<SuppliersModel> Suppliers => Set<SuppliersModel>();
    public DbSet<SuppliersProductsModel> SuppliersProducts => Set<SuppliersProductsModel>();

    public DataContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<SuppliersProductsModel>()
            .HasKey(p => new { p.ProductId, p.SupplierId });
    }
}
