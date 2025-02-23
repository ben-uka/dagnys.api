using dagnys.api.ViewModels.Order;

namespace dagnys.api.Interfaces;

public interface IOrderRepository
{
    public Task<bool> Add(OrderPostViewModel model);
    public Task<IList<OrderBaseViewModel>> List();
    public Task<OrderBaseViewModel> FindByOrderNumber(string orderNumber);
    public Task<OrderBaseViewModel> FindByOrderDate(DateTime orderDate);
}
