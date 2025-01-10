using Dagnys_API.Data;
using Dagnys_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dagnys_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    [HttpGet()]
    public async Task<ActionResult> ListProducts()
    {
        var products = await _context
            .Products.Select(products => new { products.ProductId, products.ProductName })
            .ToListAsync();

        return Ok(
            new
            {
                success = true,
                StatusCode = 200,
                data = products,
            }
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var toDelete = await _context.Products.FindAsync(id);

        _context.Products.Remove(toDelete!);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
