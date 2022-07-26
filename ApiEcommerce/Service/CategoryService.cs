using ApiEcommerce.Dtos.Category;
using ApiEcommerce.Dtos.Product;
using ApiEcommerce.Models;
using ApiEcommerce.Repository.Contract;
using ApiEcommerce.Service.Contract;
using ApiEcommerce.ServiceResponder;
using AutoMapper;

namespace ApiEcommerce.Service
{
    public class CategoryService : ICategoryService
    {
        public readonly IMapper _map;
        public readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo, IMapper map)
        {
            _map = map;
            _repo = repo;
        }
        public async Task<List<CategoryDto>> getAllCategory()
        {

            var categories = await _repo.getAllCategory();
            var CategoriesDto = new List<CategoryDto>();

            foreach (var category in categories)
            {
                CategoriesDto.Add(_map.Map<CategoryDto>(category));
            }

            return CategoriesDto;

        }

        public async Task<CategoryDto> getCategoryById(int id)
        {

            Category category = await _repo.getCategoryById(id);
            CategoryDto categoryDto = _map.Map<CategoryDto>(category);

            return categoryDto;

        }

        public async Task<List<ProductDto>> getProductListByCategoryId(int categoryId)
        {
            var products = await _repo.getProductListByCategoryId(categoryId);
            var productsDto = new List<ProductDto>();
            products.ToList().ForEach(product => { 
                ProductDto productDto = _map.Map<ProductDto>(product);
                productsDto.Add(productDto);
            });
            return productsDto;
        }

        public async Task<bool> addNewCategory(CreateCategoryDto createCategory)
        {
            var categoryDto = _map.Map<CategoryDto>(createCategory);
            var category = _map.Map<Category>(categoryDto);
            category.Id = await _repo.createIdForCategoryId();
            category.Isactive = true;
            return await _repo.addNewCategory(category);
        }

        public async Task<bool> updateCategory(int id, UpdateCategoryDto updateCategory)
        {
            var category = await _repo.getCategoryById(id);
            category.Name = updateCategory.Name;
            category.Description = updateCategory.Description;
            category.Isactive = updateCategory.Isactive;
            category.Slug = updateCategory.Slug;
            return await _repo.updateCategory(category);
        }

        public async Task<bool> deleteCategory(int id)
        {
            Category category = await _repo.getCategoryById(id);
            return await _repo.deleteCategory(category);
        }

    }
}
