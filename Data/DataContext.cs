using dagnys.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace dagnys.api.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<AddressType> AddressTypes { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<PostalAddress> PostalAddresses { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<CustomerContact> CustomerContacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerAddress>().HasKey(c => new { c.CustomerId, c.AddressId });
        modelBuilder.Entity<OrderItem>().HasKey(o => new { o.OrderItemId });
        modelBuilder.Entity<CustomerContact>().HasKey(cc => new { cc.CustomerId, cc.ContactId });

        base.OnModelCreating(modelBuilder);
    }
}
