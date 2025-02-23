using dagnys.api.ViewModels.Customer;

namespace dagnys.api.Interfaces;

public interface ICustomerRepository
{
    public Task<IList<CustomerBaseViewModel>> List();
    public Task<CustomerViewModel> Find(int id);
    public Task<bool> Add(CustomerPostViewModel model);
    public Task<bool> UpdateContact(int id, UpdateContactViewModel model);
}
