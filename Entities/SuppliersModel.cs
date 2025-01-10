using System.ComponentModel.DataAnnotations;

namespace Dagnys_API.Entities;

public class SuppliersModel
{
    [Key]
    public int SupplierId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string? Contact { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public IList<SuppliersProductsModel> SuppliersProducts { get; set; } =
        new List<SuppliersProductsModel>();
}
