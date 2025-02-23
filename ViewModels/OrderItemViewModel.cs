using dagnys.api.ViewModels.Product;

namespace dagnys.api.ViewModels;

public class OrderItemViewModel
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal TotalPrice => Quantity * PricePerUnit;
}
