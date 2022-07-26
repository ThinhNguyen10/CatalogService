using ApiEcommerce.DbCommerce;
using ApiEcommerce.Models;
using ApiEcommerce.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Repository
{
    public class CategoryRepository: ICategoryRepository 
    {
        private readonly ECOMMERCEContext _context;
        public CategoryRepository(ECOMMERCEContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Category>> getAllCategory()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> getCategoryById(int Id)
        {
            return await _context.Categories.Where(category => category.Id == Id).FirstAsync();
        }

        public async Task<ICollection<Product>> getProductListByCategoryId(int CategoryId)
        {
            return await _context.Products.Where(product => product.Category == CategoryId).ToListAsync();
        }

        public async Task<Product> getProductById(int CategoryId, int ProductId)
        {
            return await _context.Products.Where(product => product.Category == CategoryId && product.Id == ProductId).FirstAsync();
        }

        public async Task<bool> addNewCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            return await Save();
        }

        public async Task<bool> updateCategory(Category category)
        {
            _context.Categories.Update(category);
            return await Save(); 
        }

        public async Task<bool> deleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            return await Save();
        }

        private async Task<bool> Save()
        {

           int stateEntries =  await _context.SaveChangesAsync();
           return stateEntries >= 0 ? true : false;
        }

        public async Task<bool> categoryIsExists(int CategoryId)
        {
            return await _context.Categories.AnyAsync(category => category.Id == CategoryId);
        }

        public async Task<int> createIdForCategoryId()
        {
            int categoryId = 0;
            List<int> IdList = await _context.Categories.Select(category => category.Id).ToListAsync();
            for(int i = 0; i < IdList.Count; i++)
            {
                if (IdList[0] != 0){ categoryId = 0; break; }
                if (IdList[i] == IdList.Count - 1) { categoryId = IdList.Count;  break; }
                if (IdList[i+1] != IdList[i] + 1) {categoryId = IdList[i] + 1;  break; }

            }
            return categoryId;
        }
    }
}
