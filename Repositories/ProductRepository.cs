using dagnys.api.Data;
using dagnys.api.Entities;
using dagnys.api.Interfaces;
using dagnys.api.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

public class ProductRepository(DataContext context) : IProductRepository
{
    private readonly DataContext _context = context;

    public async Task<bool> AddProduct(ProductPostViewModel model)
    {
        try
        {
            if (
                await _context.Products.FirstOrDefaultAsync(p => p.ProductName == model.ProductName)
                != null
            )
            {
                throw new Exception("Produkten finns redan");
            }

            var product = new Product
            {
                ProductName = model.ProductName,
                Price = model.Price,
                Weight = model.Weight,
                Unit = model.Unit,
                BestBeforeDate = model.BestBeforeDate,
                BakedOnDate = model.BakedOnDate,
            };

            await _context.AddAsync(product);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ProductViewModel> FindById(int id)
    {
        try
        {
            var product =
                await _context.Products.Where(p => p.ProductId == id).SingleOrDefaultAsync()
                ?? throw new Exception($"Produkt med id {id} existerar inte.");

            var view = new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                Weight = product.Weight,
                Unit = product.Unit,
                BestBeforeDate = product.BestBeforeDate,
                BakedOnDate = product.BakedOnDate,
            };

            return view;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<ProductViewModel>> ListAllProducts()
    {
        try
        {
            var response = await _context.Products.ToListAsync();
            var products = response.Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Weight = p.Weight,
                Unit = p.Unit,
                BestBeforeDate = p.BestBeforeDate,
                BakedOnDate = p.BakedOnDate,
            });

            return [.. products];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> UpdatePrice(int id, PricePatchProductViewModel model)
    {
        try
        {
            var product =
                await _context.Products.FindAsync(id)
                ?? throw new Exception($"Kunde inte hitta en produkt med id {id}");

            product.Price = model.Price;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
