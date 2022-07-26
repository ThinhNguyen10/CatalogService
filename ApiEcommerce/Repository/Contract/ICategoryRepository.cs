using ApiEcommerce.Models;

namespace ApiEcommerce.Repository.Contract
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> getAllCategory();

        Task<Category> getCategoryById(int Id);

        Task<ICollection<Product>> getProductListByCategoryId(int CategoryId);

        Task<Product> getProductById(int CategoryId, int ProductId);

        Task<bool> addNewCategory(Category category);

        Task<bool> updateCategory(Category category);

        Task<bool> deleteCategory(Category category);

        Task<bool> categoryIsExists(int CategoryId);

        Task<int> createIdForCategoryId();

    }

}
