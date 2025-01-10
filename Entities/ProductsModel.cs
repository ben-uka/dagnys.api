using System.ComponentModel.DataAnnotations;

namespace Dagnys_API.Entities;

public class ProductsModel
{
    [Key]
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public IList<SuppliersProductsModel> SuppliersProducts { get; set; }
}
