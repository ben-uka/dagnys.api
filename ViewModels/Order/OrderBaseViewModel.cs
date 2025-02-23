namespace dagnys.api.ViewModels.Order;

public class OrderBaseViewModel
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
}
