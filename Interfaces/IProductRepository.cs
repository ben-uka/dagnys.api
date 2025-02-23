using dagnys.api.ViewModels.Product;

namespace dagnys.api.Interfaces;

public interface IProductRepository
{
    public Task<IList<ProductViewModel>> ListAllProducts();
    public Task<ProductViewModel> FindById(int id);
    public Task<bool> AddProduct(ProductPostViewModel model);
    public Task<bool> UpdatePrice(int id, PricePatchProductViewModel model);
}
