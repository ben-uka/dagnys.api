namespace dagnys.api.ViewModels.Order;

public class OrderPostViewModel : OrderBaseViewModel
{
    public IList<OrderItemViewModel> OrderItems { get; set; }
}
