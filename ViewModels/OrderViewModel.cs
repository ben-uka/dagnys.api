namespace dagnys.api.ViewModels;

public class OrderViewModel
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public IList<OrderItemViewModel> OrderItems { get; set; }
}
