using System.IO.Compression;
using System.Windows.Markup;
using Dagnys_API.Data;
using Dagnys_API.Entities;
using Dagnys_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dagnys_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersProductsController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    [HttpGet()]
    public async Task<IActionResult> ListProducts()
    {
        var product = await _context
            .SuppliersProducts.Include(c => c.Product)
            .Include(c => c.Supplier)
            .Select(c => new
            {
                ProduktId = c.Product.ProductId,
                ProduktNamn = c.Product.ProductName,
                LeverantörId = c.Supplier.SupplierId,
                Leverantör = c.Supplier.Name,
                ArtikelNummer = c.ArticleNumber,
                PrisPerKG = c.Price,
            })
            .ToListAsync();

        if (product != null)
        {
            return Ok(new { success = true, product });
        }
        else
            return NotFound(new { success = false, message = "Kunde inte hitta produkt." });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindSupplierProducts(int id)
    {
        var supplier = await _context
            .SuppliersProducts.Where(c => c.SupplierId == id)
            .Include(c => c.Product)
            .Select(c => new
            {
                c.Supplier.SupplierId,
                c.Supplier.Name,
                c.Supplier.Address,
                c.Product.ProductId,
                c.Product.ProductName,
                c.ArticleNumber,
                c.Price,
            })
            .ToListAsync();

        if (supplier is null)
            return NotFound(
                new
                {
                    success = false,
                    StatusCode = 404,
                    message = $"Kunde inte hitta några produkter hos leverantör ID {id}",
                }
            );
        return Ok(
            new
            {
                success = true,
                StatusCode = 200,
                data = supplier,
            }
        );
    }

    [HttpGet("product/{productName}")]
    public async Task<IActionResult> FindByProductName(string productName)
    {
        var result = await _context
            .SuppliersProducts.Include(sp => sp.Supplier)
            .Where(c => c.Product.ProductName.ToUpper().Trim() == productName.ToUpper().Trim())
            .Select(sp => new
            {
                sp.Product.ProductName,
                sp.Product.ProductId,
                sp.Supplier.Name,
                sp.Supplier.SupplierId,
                sp.Price,
                sp.ArticleNumber,
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateProductPrice(int id, [FromQuery] double price)
    {
        var prod = await _context.SuppliersProducts.FirstOrDefaultAsync(p => p.ProductId == id);

        if (prod == null)
        {
            return NotFound(
                new
                {
                    success = false,
                    message = "Produkten existerar inte.",
                    id,
                }
            );
        }
        prod.Price = price;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        return NoContent();
    }
}
