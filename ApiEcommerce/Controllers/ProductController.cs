using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiEcommerce.DbCommerce;
using ApiEcommerce.Models;

namespace ApiEcommerce.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ECOMMERCEContext _context;

        public ProductController(ECOMMERCEContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("Products")]
        public List<Product> getAllProducts()
        {
            return _context.Products.ToList(); 
        }

        [HttpPost]
        [Route("Category/{id}/Products")]
        public Product addNewProduct(int id, [FromBody] Product product)
        {
            bool categoryIsExists = _context.Categories.Any(c => c.Id == id);
            if(categoryIsExists == false)
            {
                return null;
            }
            Product newProduct = new Product() { Id = product.Id,
                Productid = product.Productid,
                Productname = product.Productname,
                Price = product.Price,
                Discount = product.Discount,
                Quantity = product.Quantity,
                Image = product.Image,
                Category = id,
                CategoryNavigation = null};
            _context.Add(newProduct);
            _context.SaveChanges();

            return newProduct;
        }

        [HttpPut]
        [Route("Category/{id}/Products/{productid}")]
        public Product updateProduct(int id, int productid, [FromBody] Product updatedproduct)
        {
            try
            {
                Product product = _context.Products.Where(c => c.Id == productid && c.Category == id).First();
                product.Productname = updatedproduct.Productname;
                product.Productname = updatedproduct.Productname;
                product.Price = updatedproduct.Price;
                product.Discount = updatedproduct.Discount;
                product.Quantity = updatedproduct.Quantity;
                product.Image = updatedproduct.Image;
                _context.SaveChanges();
                return updatedproduct;

            }
            catch(Exception ex)
            {
                return null;
            }
           
        }

        [HttpDelete]
        [Route("Category/{id}/Products/{productid}")]
        public Product deleteProduct(int id, int productid)
        {
            try
            {
                Product product = _context.Products.Where(c => c.Id == productid && c.Category == id).First();
                _context.Products.Remove(product);
                _context.SaveChanges();
                return product;

            }
            catch (Exception ex)
            {
                return null;
            }
            


        }
    }
}
