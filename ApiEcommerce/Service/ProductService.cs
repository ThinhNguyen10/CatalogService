using ApiEcommerce.Dtos.Product;
using ApiEcommerce.Filter;
using ApiEcommerce.Models;
using ApiEcommerce.Repository.Contract;
using ApiEcommerce.Service.Contract;
using AutoMapper;

namespace ApiEcommerce.Service
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _map;
        public ProductService(IProductRepository repo, IMapper map)
        {
            _repo = repo;
            _map = map;
        }
        public async Task<List<ProductDto>> getAllProduct(PaginationFilter paginationFilter)
        {
            var productsDto = new List<ProductDto>();

            int pageNumber = paginationFilter.PageNumber;
            int pageSize = paginationFilter.PageSize;
            var products = await _repo.getAllProduct(pageNumber, pageSize);
            products.ToList().ForEach(product =>
            {
                ProductDto productDto = _map.Map<ProductDto>(product);
                productsDto.Add(productDto);
            });
            return productsDto;
        }

        public async Task<bool> addNewProduct(CreateProductDto createProduct)
        {
            ProductDto productDto= _map.Map<ProductDto>(createProduct);
            Product product = _map.Map<Product>(productDto);
            product.Id = await _repo.createIdForProductId();
            return await _repo.addNewProduct(product);
        }

        public async Task<bool> updateProduct(int id, UpdateProductDto updateProduct)
        {
            Product product = await _repo.getProductById(id);
            product.Productname = updateProduct.Productname;
            product.Quantity = updateProduct.Quantity;
            product.Price = updateProduct.Price;
            product.Discount = updateProduct.Discount;
            product.Image = updateProduct.Image;

            return await _repo.updateProduct(product);
        }
        public async Task<bool> deleteProduct(int id)
        {
            var product = await _repo.getProductById(id);
            return await _repo.deleteProduct(product);
        }
    }
}
