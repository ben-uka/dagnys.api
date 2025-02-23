using System.ComponentModel.DataAnnotations;

namespace dagnys.api.ViewModels.Product;

public class ProductPostViewModel
{
    [Required(ErrorMessage = "Produktnamn måste anges.")]
    public string ProductName { get; set; }

    [Required(ErrorMessage = "Pris måste anges.")]
    public decimal Price { get; set; }
    public double Weight { get; set; }
    public string Unit { get; set; }
    public DateTime BestBeforeDate { get; set; }
    public DateTime BakedOnDate { get; set; }
}
