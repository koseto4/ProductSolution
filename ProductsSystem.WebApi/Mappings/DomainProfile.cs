using AutoMapper;
using ProductsSystem.Models.EntityModels;
using ProductsSystem.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsSystem.WebApi.Mappings
{
    public class DomainProfile: Profile
    {
        public DomainProfile()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>();

            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();

            CreateMap<SubCategory, SubCategoryViewModel>();
            CreateMap<SubCategoryViewModel, SubCategory>();
        }

    }
}
