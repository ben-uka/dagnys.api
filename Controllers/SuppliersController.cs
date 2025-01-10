using Dagnys_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dagnys_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    [HttpGet()]
    public async Task<ActionResult> ListSuppliers()
    {
        var suppliers = await _context
            .Suppliers.Select(supplier => new
            {
                supplier.SupplierId,
                supplier.Name,
                supplier.Contact,
                supplier.Address,
                supplier.Phone,
                supplier.Email,
            })
            .ToListAsync();

        return Ok(
            new
            {
                success = true,
                StatusCode = 200,
                data = suppliers,
            }
        );
    }
}
