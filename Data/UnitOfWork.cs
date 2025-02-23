using dagnys.api.Interfaces;
using dagnys.api.Repositories;

namespace dagnys.api.Data;

public class UnitOfWork(DataContext context, IAddressRepository repo) : IUnitOfWork
{
    private readonly DataContext _context = context;
    private readonly IAddressRepository _repo = repo;

    public IAddressRepository AddressRepository => new AddressRepository(_context);

    public ICustomerRepository CustomerRepository => new CustomerRepository(_context, _repo);

    public IOrderRepository OrderRepository => new OrderRepository(_context);

    public IProductRepository ProductRepository => new ProductRepository(_context);

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }
}
