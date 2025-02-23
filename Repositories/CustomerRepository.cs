using dagnys.api.Data;
using dagnys.api.Entities;
using dagnys.api.Interfaces;
using dagnys.api.ViewModels;
using dagnys.api.ViewModels.Address;
using dagnys.api.ViewModels.Customer;
using Microsoft.EntityFrameworkCore;

namespace dagnys.api.Repositories;

public class CustomerRepository(DataContext context, IAddressRepository repo) : ICustomerRepository
{
    private readonly DataContext _context = context;
    private readonly IAddressRepository _repo = repo;

    public async Task<bool> Add(CustomerPostViewModel model)
    {
        try
        {
            if (
                await _context.Customers.FirstOrDefaultAsync(c =>
                    c.Email.ToLower().Trim() == model.Email.ToLower().Trim()
                ) != null
            )
            {
                throw new Exception($"Kund med mejladress {model.Email} finns redan.");
            }

            var customer = new Customer
            {
                StoreName = model.StoreName,
                Phone = model.Phone,
                Email = model.Email,
            };

            await _context.AddAsync(customer);

            foreach (var a in model.Addresses)
            {
                var address = await _repo.Add(a);

                await _context.CustomerAddresses.AddAsync(
                    new CustomerAddress { Address = address, Customer = customer }
                );
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<CustomerViewModel> Find(int id)
    {
        try
        {
            var customer =
                await _context
                    .Customers.Where(c => c.Id == id)
                    .Include(ca => ca.CustomerAddresses)
                    .ThenInclude(a => a.Address)
                    .ThenInclude(p => p.PostalAddress)
                    .Include(ca => ca.CustomerAddresses)
                    .ThenInclude(a => a.Address)
                    .ThenInclude(at => at.AddressType)
                    .Include(o => o.Orders)
                    .ThenInclude(oi => oi.OrderItems)
                    .ThenInclude(p => p.Product)
                    .Include(cc => cc.CustomerContacts)
                    .ThenInclude(c => c.Contact)
                    .SingleOrDefaultAsync()
                ?? throw new Exception($"Det finns ingen kund med id {id}");

            var view = new CustomerViewModel
            {
                Id = customer.Id,
                StoreName = customer.StoreName,
                Phone = customer.Phone,
                Email = customer.Email,
            };

            var contacts = customer.CustomerContacts.Select(c => new ContactViewModel
            {
                ContactId = c.Contact.Id,
                FirstName = c.Contact.FirstName,
                LastName = c.Contact.LastName,
                Email = c.Contact.Email,
            });

            view.Contacts = [.. contacts];

            var addresses = customer.CustomerAddresses.Select(c => new AddressViewModel
            {
                AddressLine = c.Address.AddressLine,
                PostalCode = c.Address.PostalAddress.ZipCode,
                City = c.Address.PostalAddress.City,
                AddressType = c.Address.AddressType.Value,
            });

            view.Addresses = [.. addresses];

            var orders = customer.Orders.Select(c => new OrderViewModel
            {
                OrderId = c.OrderId,
                OrderNumber = c.OrderNumber,
                OrderDate = c.OrderDate,
                OrderItems =
                [
                    .. c.OrderItems.Select(c => new OrderItemViewModel
                    {
                        OrderId = c.OrderId,
                        OrderItemId = c.OrderItemId,
                        ProductId = c.ProductId,
                        Quantity = c.Quantity,
                        PricePerUnit = c.PricePerUnit,
                    }),
                ],
            });

            view.Orders = [.. orders];

            return view;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<CustomerBaseViewModel>> List()
    {
        try
        {
            var response = await _context.Customers.ToListAsync(); // Endast hämta kunderna

            var customers = response
                .Select(c => new CustomerBaseViewModel
                {
                    Id = c.Id,
                    StoreName = c.StoreName,
                    Email = c.Email,
                    Phone = c.Phone,
                })
                .ToList();

            return customers;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> UpdateContact(int id, UpdateContactViewModel model)
    {
        var contact =
            await _context.Contacts.FindAsync(id)
            ?? throw new Exception($"Vi kunde inte hitta någon med id {id}");

        contact.FirstName = model.FirstName;
        contact.LastName = model.LastName;
        contact.Email = model.Email;

        _context.Contacts.Update(contact);

        if (_context.ChangeTracker.HasChanges())
        {
            await _context.SaveChangesAsync();
        }

        return true;
    }
}
