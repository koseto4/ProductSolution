using AutoMapper;
using ProductsSystem.Data.Core;
using ProductsSystem.Models.EntityModels;
using ProductsSystem.Services.Services.Interfaces;
using ProductsSystem.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsSystem.Services.Services
{
    public class ProductService : IProductService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Create(ProductViewModel viewModel)
        {
            var result = true;
            try
            {
                var entity = _mapper.Map<Product>(viewModel);
                await _unitOfWork.ProductRepository.Add(entity);
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
                await _unitOfWork.ProductRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return result;
            }
            catch
            {
                result = false;
                return result;
            }
        }

        public async Task<bool> Edit(ProductViewModel viewModel)
        {
            var result = true;
            try
            {
                var entity = _mapper.Map<Product>(viewModel);
                await _unitOfWork.ProductRepository.UpdateAsync(entity.Id, entity);
                await _unitOfWork.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                return result;
            }
        }

        public async Task<IEnumerable<ProductViewModel>> GetAll()
        {
            var entities = await _unitOfWork.ProductRepository.GetAllAsync();

            var result = _mapper.Map< IEnumerable <Product> ,IEnumerable <ProductViewModel>>(entities).ToList();

            return result;
        }

        public async Task<ProductViewModel> GetbyId(int id)
        {
            var entity = await _unitOfWork.ProductRepository.GetByIdAsync(id);           
            var viewModel = _mapper.Map<ProductViewModel>(entity);
            return viewModel;
        }

        public async Task<IEnumerable<ProductViewModel>> GetByName(string searchName)
        {
            var entities = await _unitOfWork.ProductRepository.GetAllAsync();
            if (searchName != null)
            {
                entities = entities.Where(x => x.Name.Equals(searchName));
            }
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(entities).ToList(); ;
            return result;
        }
    }
}
