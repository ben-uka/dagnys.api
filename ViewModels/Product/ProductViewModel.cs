using System.ComponentModel.DataAnnotations;

namespace dagnys.api.ViewModels.Product;

public class ProductViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public double Weight { get; set; }
    public string Unit { get; set; }
    public DateTime BestBeforeDate { get; set; }
    public DateTime BakedOnDate { get; set; }
}
