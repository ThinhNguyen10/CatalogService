using ApiEcommerce.Dtos.Category;
using ApiEcommerce.Dtos.Product;
using ApiEcommerce.ServiceResponder;

namespace ApiEcommerce.Service.Contract
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> getAllCategory();

        Task<CategoryDto> getCategoryById(int id);

        Task<List<ProductDto>> getProductListByCategoryId(int categoryId);

        Task<bool> addNewCategory(CreateCategoryDto createCategory);

        Task<bool> updateCategory(int id, UpdateCategoryDto updateCategory);

        Task<bool> deleteCategory(int id);
    }
}
