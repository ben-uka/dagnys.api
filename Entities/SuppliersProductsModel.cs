using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dagnys_API.Entities;

public class SuppliersProductsModel
{
    public int ProductId { get; set; }
    public int SupplierId { get; set; }
    public string ArticleNumber { get; set; }
    public double Price { get; set; }
    public ProductsModel Product { get; set; }
    public SuppliersModel Supplier { get; set; }
}
