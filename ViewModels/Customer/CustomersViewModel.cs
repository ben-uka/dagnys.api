using dagnys.api.ViewModels.Address;

namespace dagnys.api.ViewModels.Customer;

public class CustomersViewModel
{
    public int Id { get; set; }
    public string StoreName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public IList<ContactViewModel> Contacts { get; set; }
    public IList<AddressViewModel> Addresses { get; set; }
}
