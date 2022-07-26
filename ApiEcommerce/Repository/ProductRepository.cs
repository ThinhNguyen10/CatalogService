using ApiEcommerce.Models;
using ApiEcommerce.Repository.Contract;
using ApiEcommerce.DbCommerce;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECOMMERCEContext _context;

        public ProductRepository(ECOMMERCEContext context)
        {
            _context = context;
        }
        public async Task<bool> addNewProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            return await Save();
        }

        public async Task<bool> deleteProduct(Product product)
        {
            _context.Products.Remove(product);
            return await Save();
        }

        public async Task<ICollection<Product>> getAllProduct(int pageNumber, int pageSize)
        {

            return await _context.Products
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();
        }

        public async Task<Product> getProductById(int id)
        {
            return await _context.Products.Where(product => product.Id == id).FirstAsync();
        }

        public async Task<bool> updateProduct(Product product)
        {
            _context.Products.Update(product);
            return await Save();
        }
        private async Task<bool> Save()
        {

            int stateEntries = await _context.SaveChangesAsync();
            return stateEntries >= 0 ? true : false;
        }

        public async Task<int> createIdForProductId()
        {
            int productId = 0;
            List<int> IdList = await _context.Products.Select(product => product.Id).ToListAsync();
            for (int i = 0; i < IdList.Count; i++)
            {
                if (IdList[0] != 1) { productId = 1; break; }
                if (IdList[i] == IdList.Count) { productId = IdList.Count + 1; break; }
                if (IdList[i + 1] != IdList[i] + 1) { productId = IdList[i] + 1; break; }

            }
            return productId;
        }

        public async Task<int> getTotalRecordInProducs()
        {
            return await _context.Products.CountAsync();
        }
    }
}
