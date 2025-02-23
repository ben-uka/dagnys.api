using System.Text.Json;
using dagnys.api.Entities;
using dagnys.api.ViewModels.Address;
using dagnys.api.ViewModels.Customer;
using Microsoft.EntityFrameworkCore;

namespace dagnys.api.Data;

public static class Seed
{
    private static readonly JsonSerializerOptions options =
        new() { PropertyNameCaseInsensitive = true };

    public static async Task LoadAddressTypes(DataContext context)
    {
        if (context.AddressTypes.Any())
            return;

        var json = await File.ReadAllTextAsync("Data/json/addresstype.json");
        var addressTypes = JsonSerializer.Deserialize<List<AddressType>>(json, options);

        if (addressTypes is not null && addressTypes.Count > 0)
        {
            await context.AddressTypes.AddRangeAsync(addressTypes);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadProducts(DataContext context)
    {
        if (context.Products.Any())
            return;

        var json = File.ReadAllText("Data/json/product.json");
        var products = JsonSerializer.Deserialize<List<Product>>(json, options);

        if (products is not null && products.Count > 0)
        {
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadOrders(DataContext context)
    {
        if (context.Orders.Any())
            return;

        var json = File.ReadAllText("Data/json/order.json");
        var orders = JsonSerializer.Deserialize<List<Order>>(json, options);

        if (orders is not null && orders.Count > 0)
        {
            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadOrderItems(DataContext context)
    {
        if (context.OrderItems.Any())
            return;

        var json = File.ReadAllText("Data/json/orderitem.json");
        var orderItems = JsonSerializer.Deserialize<List<OrderItem>>(json, options);

        if (orderItems is not null && orderItems.Count > 0)
        {
            await context.OrderItems.AddRangeAsync(orderItems);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadPostalAddresses(DataContext context)
    {
        if (context.PostalAddresses.Any())
            return;

        var json = await File.ReadAllTextAsync("Data/json/postaladdress.json");
        var postalAddresses = JsonSerializer.Deserialize<List<PostalAddress>>(json, options);

        if (postalAddresses is not null && postalAddresses.Count > 0)
        {
            await context.PostalAddresses.AddRangeAsync(postalAddresses);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadCustomers(DataContext context)
    {
        if (context.Customers.Any())
            return;

        var json = await File.ReadAllTextAsync("Data/json/customer.json");
        var customers = JsonSerializer.Deserialize<List<CustomerPostViewModel>>(json, options);

        if (customers is not null && customers.Count > 0)
        {
            foreach (var customerViewModel in customers)
            {
                var newCustomer = new Customer
                {
                    StoreName = customerViewModel.StoreName,
                    Email = customerViewModel.Email,
                    Phone = customerViewModel.Phone,
                };

                await context.Customers.AddAsync(newCustomer);

                foreach (var addressViewModel in customerViewModel.Addresses)
                {
                    var postalAddress = await context.PostalAddresses.FirstOrDefaultAsync(pa =>
                        pa.ZipCode.Replace(" ", "").Trim()
                            == addressViewModel.ZipCode.Replace(" ", "").Trim()
                        && pa.City.Trim().ToLower() == addressViewModel.City.Trim().ToLower()
                    );

                    postalAddress ??= new PostalAddress
                    {
                        ZipCode = addressViewModel.ZipCode.Replace(" ", "").Trim(),
                        City = addressViewModel.City.Trim(),
                    };

                    var address = await context.Addresses.FirstOrDefaultAsync(a =>
                        a.AddressLine.Trim().ToLower()
                            == addressViewModel.AddressLine.Trim().ToLower()
                        && a.PostalAddressId == postalAddress.Id
                    );

                    address ??= new Address
                    {
                        AddressLine = addressViewModel.AddressLine,
                        AddressTypeId = (int)addressViewModel.AddressType,
                        PostalAddress = postalAddress,
                    };

                    var customerAddress = new CustomerAddress
                    {
                        Customer = newCustomer,
                        Address = address,
                    };

                    await context.CustomerAddresses.AddAsync(customerAddress);
                }

                await context.SaveChangesAsync();
            }
        }
    }

    public static async Task LoadContacts(DataContext context)
    {
        if (context.Contacts.Any())
            return;

        var json = await File.ReadAllTextAsync("Data/json/contact.json");
        var contacts = JsonSerializer.Deserialize<List<Contact>>(json, options);

        if (contacts is not null && contacts.Count > 0)
        {
            await context.Contacts.AddRangeAsync(contacts);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadCustomerContacts(DataContext context)
    {
        if (context.CustomerContacts.Any())
            return;

        var json = await File.ReadAllTextAsync("Data/json/customercontact.json");
        var customerContacts = JsonSerializer.Deserialize<List<CustomerContact>>(json, options);

        if (customerContacts is not null && customerContacts.Count > 0)
        {
            await context.CustomerContacts.AddRangeAsync(customerContacts);
            await context.SaveChangesAsync();
        }
    }
}
