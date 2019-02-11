using ProductsSystem.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductsSystem.Services.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductViewModel> GetbyId(int id);
        Task<IEnumerable<ProductViewModel>> GetAll();
        Task<IEnumerable<ProductViewModel>> GetByName(string searchName);
        Task<bool> Create(ProductViewModel viewModel);
        Task<bool> Edit(ProductViewModel viewModel);
        Task<bool> Delete(int id);
    }
}
