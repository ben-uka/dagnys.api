using System.Text.Json;
using Dagnys_API.Entities;

namespace Dagnys_API.Data;

public static class Seed
{
    public static async Task LoadProducts(DataContext context)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        if (context.Products.Any())
            return;

        var json = File.ReadAllText("Data/json/products.json");
        var products = JsonSerializer.Deserialize<List<ProductsModel>>(json, options);

        if (products is not null && products.Count > 0)
        {
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadSuppliers(DataContext context)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        if (context.Suppliers.Any())
            return;

        var json = File.ReadAllText("Data/json/suppliers.json");
        var suppliers = JsonSerializer.Deserialize<List<SuppliersModel>>(json, options);

        if (suppliers is not null && suppliers.Count > 0)
        {
            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadSupplierProducts(DataContext context)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        if (context.SuppliersProducts.Any())
            return;

        var json = File.ReadAllText("Data/json/suppliersproducts.json");
        var supplierproducts = JsonSerializer.Deserialize<List<SuppliersProductsModel>>(
            json,
            options
        );

        if (supplierproducts is not null && supplierproducts.Count > 0)
        {
            await context.SuppliersProducts.AddRangeAsync(supplierproducts);
            await context.SaveChangesAsync();
        }
    }
}
