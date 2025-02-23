namespace dagnys.api.Interfaces;

public interface IUnitOfWork
{
    IAddressRepository AddressRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    IOrderRepository OrderRepository { get; }
    IProductRepository ProductRepository { get; }
    Task<bool> Complete();
    bool HasChanges();
}
