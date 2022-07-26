using ApiEcommerce.Dtos.Category;
using ApiEcommerce.Dtos.Product;
using ApiEcommerce.Models;
using AutoMapper;

namespace ApiEcommerce.Mapper
{
    public class DtoMapping : Profile
    {
        public DtoMapping()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, CategoryDto>().ReverseMap();    
            CreateMap<Product,ProductDto>().ReverseMap();
            CreateMap<CreateProductDto, ProductDto>().ReverseMap();

            
        }
    }
}
