using dagnys.api.Data;
using dagnys.api.Entities;
using dagnys.api.Interfaces;
using dagnys.api.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnys.api.Repositories;

public class OrderRepository(DataContext context) : IOrderRepository
{
    private readonly DataContext _context = context;

    public async Task<bool> Add(OrderPostViewModel model)
    {
        if (await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == model.OrderId) != null)
        {
            throw new Exception($"Ordern finns redan.");
        }

        var order = new Order
        {
            CustomerId = model.CustomerId,
            OrderNumber = model.OrderNumber,
            OrderDate = model.OrderDate,
            OrderItems =
            [
                .. model.OrderItems.Select(o => new OrderItem
                {
                    OrderItemId = o.OrderItemId,
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                    PricePerUnit = o.PricePerUnit,
                }),
            ],
        };

        await _context.AddAsync(order);

        if (_context.ChangeTracker.HasChanges())
        {
            await _context.SaveChangesAsync();
        }

        return true;
    }

    public async Task<OrderBaseViewModel> FindByOrderDate(DateTime orderDate)
    {
        try
        {
            var order =
                await _context
                    .Orders.Where(o => o.OrderDate == orderDate)
                    .Select(o => new OrderBaseViewModel
                    {
                        OrderId = o.OrderId,
                        CustomerId = o.CustomerId,
                        OrderNumber = o.OrderNumber,
                        OrderDate = o.OrderDate,
                    })
                    .FirstOrDefaultAsync()
                ?? throw new Exception(
                    "Beställning med det angivna ordernumret eller orderdatumet kan inte hittas."
                );

            return order;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<OrderBaseViewModel> FindByOrderNumber(string orderNumber)
    {
        try
        {
            var order =
                await _context
                    .Orders.Where(o => o.OrderNumber == orderNumber)
                    .Select(o => new OrderBaseViewModel
                    {
                        OrderId = o.OrderId,
                        CustomerId = o.CustomerId,
                        OrderNumber = o.OrderNumber,
                        OrderDate = o.OrderDate,
                    })
                    .FirstOrDefaultAsync()
                ?? throw new Exception(
                    "Beställning med det angivna ordernumret eller orderdatumet kan inte hittas."
                );

            return order;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<OrderBaseViewModel>> List()
    {
        try
        {
            var response = await _context.Orders.ToListAsync();
            var orders = response.Select(o => new OrderBaseViewModel
            {
                OrderId = o.OrderId,
                CustomerId = o.CustomerId,
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
            });

            return [.. orders];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
