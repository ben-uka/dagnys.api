using dagnys.api.Entities;
using dagnys.api.ViewModels.Address;

namespace dagnys.api.Interfaces;

public interface IAddressRepository
{
    public Task<Address> Add(AddressPostViewModel model);
}
