using Dagnys_API.Controllers;
using Dagnys_API.Entities;

namespace Dagnys_API.ViewModels;

public class SuppliersProductsViewModel
{
    public IList<ProductViewModel> Products { get; set; }
}
