using ApiEcommerce.DbCommerce;
using ApiEcommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiEcommerce.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ECOMMERCEContext _context;

        public CategoryController(ECOMMERCEContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Category/All")]
        public List<Category> getAllCategory()
        {
            return _context.Categories.ToList();
        }

        [HttpGet]
        [Route("Category/{id}")]
        public Category getCategoryById(int id)
        {
            int amountOfCategories = _context.Categories.Count();


            if (id > amountOfCategories || id < 0)
            {
                return null;
            }
            else
            {
                return _context.Categories.Where(category => category.Id == id).First();
            }
        }

        [HttpGet]
        [Route("Category/{id}/Products")]
        public List<Product> getProductListByCategoryId(int id)
        {
            int amountOfCategories = _context.Categories.Count();


            if (id > amountOfCategories || id < 0)
            {
                return null;
            }
            else
            {
                return _context.Products.Where(product => product.Category == id).ToList();
            }
        }

        [HttpGet]
        [Route("Category/{id}/Products/{productid}")]
        public Product getProductById(int id,int productid)
        {
            int amountOfCategories = _context.Categories.Count();
            bool isExists = _context.Products.Any(product => product.Category == id && product.Id == productid);

            if (id > amountOfCategories || id < 0 || isExists  == false)
            {
                return null;
            }
            else
            {
                return _context.Products.Where(product => product.Category == id && product.Id == productid).First();
            }
        }

        [HttpPost]
        [Route("Category")]
        public Category addNewCategory([FromBody] Category catg)
        {
            string categoryName = catg.Name;
            string categorySlug = catg.Slug;
            string categoryDescription = catg.Description;
            
            if(String.IsNullOrEmpty(categoryName) || String.IsNullOrEmpty(categorySlug) || String.IsNullOrEmpty(categoryDescription) )
            {
                return null;
            }

            _context.Categories.Add(catg);
            _context.SaveChanges();
            return catg;
        }

        [HttpPut]
        [Route("Category/{id}")]
        public Category updateCategory(int id, [FromBody] Category catg)
        {
            try
            {

                Category category = _context.Categories.Where(category => category.Id == id).First();
                category.Name = catg.Name;
                category.Slug = catg.Slug;
                category.Description = catg.Description;
                category.Isactive = catg.Isactive;

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                return null;
            }
           
            return catg;

        }


        [HttpDelete]
        [Route("Category/{id}")]

        public Category deleteCategoryById(int id)
        {
            try
            {
                Category catg = _context.Categories.Where(category => category.Id == id).First();
                bool isExistsInProducts = _context.Products.Any(product => product.Category == id);
                if(isExistsInProducts == true)
                {
                    return null;
                }
                _context.Categories.Remove(catg);
                _context.SaveChanges();
                return catg;
            }
            catch (Exception ex)
            {
                return null;
            }
            
            
        }
    }
}
