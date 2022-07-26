using ApiEcommerce.Models;

namespace ApiEcommerce.Repository.Contract
{
    public interface IProductRepository
    {
        Task<ICollection<Product>> getAllProduct(int pageNumber, int pageSize);

        Task<Product> getProductById(int id);

        Task<bool> addNewProduct(Product product);

        Task<bool> updateProduct(Product product);

        Task<bool> deleteProduct(Product product);

        Task<int> createIdForProductId();

        Task<int> getTotalRecordInProducs();
    }
}
