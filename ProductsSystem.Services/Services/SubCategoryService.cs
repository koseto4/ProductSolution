using ProductsSystem.Services.Services.Interfaces;
using ProductsSystem.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using AutoMapper;
using System.Threading.Tasks;
using ProductsSystem.Data.Core;
using ProductsSystem.Models.EntityModels;
using System.Linq;

namespace ProductsSystem.Services.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Create(SubCategoryViewModel viewModel)
        {
            var result = true;
            try
            {
                var entity = _mapper.Map<SubCategory>(viewModel);
                await _unitOfWork.SubCategoryRepository.Add(entity);
                await _unitOfWork.SaveChangesAsync();
                return result;
            }
            catch
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
               
                var allProducts = await _unitOfWork.ProductRepository.GetAllAsync();                
                var products = allProducts.Where(x => x.SubCategoryId== id).ToList();
                products.ForEach(x => _unitOfWork.ProductRepository.DeleteById(x.Id));
                await _unitOfWork.SubCategoryRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return result;
            }
            catch
            {
                result = false;
                return result;
            }
        }

        public async Task<bool> Edit(SubCategoryViewModel viewModel)
        {
            var result = true;
            try
            {
                var entity = _mapper.Map<SubCategory>(viewModel);
                await _unitOfWork.SubCategoryRepository.UpdateAsync(entity.Id, entity);
                await _unitOfWork.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                return result;
            }
        }

        public async Task<IEnumerable<SubCategoryViewModel>> GetAll()
        {
            var entities = await _unitOfWork.SubCategoryRepository.GetAllAsync();

            var result = _mapper.Map<IEnumerable<SubCategory>, IEnumerable<SubCategoryViewModel>>(entities).ToList();
           
            return result;
        }

        public async Task<SubCategoryViewModel> GetbyId(int id)
        {
            var entity = await _unitOfWork.SubCategoryRepository.GetByIdAsync(id);
            var viewModel= _mapper.Map<SubCategoryViewModel>(entity);
            return viewModel;
        }

        public async Task<IEnumerable<SubCategoryViewModel>> GetByName(string searchName)
        {
            var entities = await _unitOfWork.SubCategoryRepository.GetAllAsync();
            if (searchName != null)
            {
                entities = entities.Where(x => x.Name.Equals(searchName));
            }
            var result = _mapper.Map<IEnumerable<SubCategory>, IEnumerable<SubCategoryViewModel>>(entities).ToList();
            return result;
        }
    }
}
