using ProductsSystem.Models.EntityModels;
using ProductsSystem.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsSystem.Services.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryViewModel> GetbyId(int id);
        Task<IEnumerable<CategoryViewModel>> GetAll();
        Task<IEnumerable<CategoryViewModel>> GetByName(string searchName);
        Task<bool> Create(CategoryViewModel viewModel);
        Task<bool> Edit(CategoryViewModel viewModel);
        Task<bool> Delete(int id);
       
    }
}
