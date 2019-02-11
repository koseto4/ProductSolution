using AutoMapper;
using ProductsSystem.Data.Core;
using ProductsSystem.Models.EntityModels;
using ProductsSystem.Services.Services.Interfaces;
using ProductsSystem.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsSystem.Services
{
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryViewModel> GetbyId(int id)
        {
            var entity = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            var viewModel = _mapper.Map<CategoryViewModel>(entity);
            return viewModel;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAll()
        {
            var entities = await _unitOfWork.CategoryRepository.GetAllAsync();            
            var result = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(entities);          
            return result;
        }

        public async Task<bool> Create(CategoryViewModel viewModel)
        {
            var result = true;
            try
            {
                var entity = _mapper.Map<Category>(viewModel);
                await _unitOfWork.CategoryRepository.Add(entity);
                await _unitOfWork.SaveChangesAsync();
                return result;
            }
            catch
            {
                result = false;
                return result;
            }
        }

        public async Task<bool> Edit(CategoryViewModel viewModel)
        {
            var result = true;
            try
            {
                var entity = _mapper.Map<Category>(viewModel);
                await _unitOfWork.CategoryRepository.UpdateAsync(entity.ID, entity);
                await _unitOfWork.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                return result;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var result = true;
            try
            {
                var subCategories = await _unitOfWork.SubCategoryRepository.GetAllAsync();
                var allProducts = await _unitOfWork.ProductRepository.GetAllAsync();
                var subCategoriesByCategory = subCategories.Where(x => x.CategoryId == id).ToList();
                var products = allProducts.Where(x => subCategoriesByCategory.Select(y => y.Id).Contains(x.SubCategoryId)).ToList();
                products.ForEach(x => _unitOfWork.ProductRepository.DeleteById(x.Id));
                subCategoriesByCategory.ForEach(x => _unitOfWork.SubCategoryRepository.DeleteById(x.Id));
                await _unitOfWork.CategoryRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return result;
            }
            catch
            {
                result = false;
                return result;
            }
        }

        public async Task<IEnumerable<CategoryViewModel>> GetByName(string searchName)
        {
            var entities = await _unitOfWork.CategoryRepository.GetAllAsync();
            if (searchName != null)
            {
                entities = entities.Where(x => x.Name.Equals(searchName));
            }
            var result = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(entities).ToList();
            
            return result;
        }
    }
}
