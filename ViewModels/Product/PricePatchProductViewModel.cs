using System.ComponentModel.DataAnnotations;

namespace dagnys.api.ViewModels.Product;

public class PricePatchProductViewModel
{
    [Required(ErrorMessage = "Pris måste anges.")]
    public decimal Price { get; set; }
}
