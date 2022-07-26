using ApiEcommerce.Dtos.Product;
using ApiEcommerce.Filter;

namespace ApiEcommerce.Service.Contract
{
    public interface IProductService
    {
        Task<List<ProductDto>> getAllProduct(PaginationFilter paginationFilter);

        Task<bool> addNewProduct(CreateProductDto createProduct);

        Task<bool> updateProduct(int id, UpdateProductDto updateProduct);


        Task<bool> deleteProduct(int id);
    }
}
