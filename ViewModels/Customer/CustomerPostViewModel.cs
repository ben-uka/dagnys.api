using dagnys.api.ViewModels.Address;

namespace dagnys.api.ViewModels.Customer;

public class CustomerPostViewModel : CustomerBaseViewModel
{
    public IList<AddressPostViewModel> Addresses { get; set; }
}
