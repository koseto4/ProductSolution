using ProductsSystem.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductsSystem.Services.Services.Interfaces
{
    public interface ISubCategoryService
    {
        Task<SubCategoryViewModel> GetbyId(int id);
        Task<IEnumerable<SubCategoryViewModel>> GetAll();
        Task<IEnumerable<SubCategoryViewModel>> GetByName(string searchName);
        Task<bool> Create(SubCategoryViewModel viewModel);
        Task<bool> Edit(SubCategoryViewModel viewModel);
        Task<bool> Delete(int id);
        
    }
}
