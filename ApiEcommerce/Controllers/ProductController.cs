using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiEcommerce.DbCommerce;
using ApiEcommerce.Models;
using ApiEcommerce.ServiceResponder;
using ApiEcommerce.Dtos.Product;
using ApiEcommerce.Service.Contract;
using ApiEcommerce.Repository.Contract;
using ApiEcommerce.Filter;
using ApiEcommerce.Helpers;

namespace ApiEcommerce.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _serv;
        private readonly ICategoryRepository _repoCategory;
        private readonly IProductRepository _repoProduct;
        private readonly IUriService _uriService;

        public ProductController(IProductService serv, ICategoryRepository repoCategory, IProductRepository repoProduct, IUriService uriService)
        {
            _serv = serv;  
            _repoCategory = repoCategory;
            _repoProduct = repoProduct;
            _uriService = uriService;
        }


        [HttpGet]
        [Route("Products")]
        public async Task<IActionResult> getAllProducts([FromQuery] PaginationFilter paginationFilter)
        {
            try
            {
                string route = Request.Path.Value;
                var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
                var products = await _serv.getAllProduct(validPaginationFilter);
                var totalRecords = await _repoProduct.getTotalRecordInProducs();
                var res = PaginationHelper.CreatePagedReponse(products, validPaginationFilter, totalRecords, _uriService, route);
                return Ok(res);

            }
            catch (Exception ex)
            {
                ServiceResponse<CreateProductDto> res = new ServiceResponse<CreateProductDto>();
                res.Data = null;
                res.Message = "Error";
                res.StatusCode = StatusCodes.Status500InternalServerError.ToString();
                res.Success = false;
                res.ErrorMessages = new List<string>() { "Category id not exists. Please enter correct id", ex.Message.ToString() };
                return StatusCode(500,res);
            }

        }

        [HttpPost]
        [Route("Products")]
        public async Task<IActionResult> addNewProduct([FromBody] CreateProductDto createProduct)
        {
            ServiceResponse<CreateProductDto> res = new ServiceResponse<CreateProductDto>();
            var isExists = await _repoCategory.categoryIsExists(Convert.ToInt32(createProduct.Category));
            if (isExists == false)
            {
                res.Data = null;
                res.Message = "Error";
                res.StatusCode = StatusCodes.Status400BadRequest.ToString();
                res.Success = false;
                res.ErrorMessages = new List<string>(){ "Category id not exists. Please enter correct id"};
                return BadRequest(res);
            }


            string productId = createProduct.Productid;
            string productName = createProduct.Productname;
            if(String.IsNullOrEmpty(productId) || String.IsNullOrEmpty(productName))
            {
                res.Data = null;
                res.Message = "Error";
                res.StatusCode = StatusCodes.Status400BadRequest.ToString();
                res.Success = false;
                res.ErrorMessages = new List<string>() { "productId and productName is not null or empty" };
                return BadRequest(res);
            }

            bool isSuccess = await _serv.addNewProduct(createProduct);
            if (!isSuccess)
            {
                res.Data = null;
                res.Message = "Error";
                res.StatusCode = StatusCodes.Status400BadRequest.ToString();
                res.Success = false;
                res.ErrorMessages = new List<string>() { "Not add product succesfully" };
                return NotFound(res);
            }


            res.Data = createProduct;
            res.Message = "Success";
            res.StatusCode = StatusCodes.Status201Created.ToString();
            res.Success = true;
            return Ok(res);
        }

        [HttpPut]
        [Route("Products/{id}")]
        public async Task<IActionResult> updateProduct(int id, [FromBody] UpdateProductDto updateProduct)
        {

            ServiceResponse<UpdateProductDto> res = new ServiceResponse<UpdateProductDto>();
            string productName = updateProduct.Productname;
            if (String.IsNullOrEmpty(productName))
            {
                res.Data = updateProduct;
                res.Success = false;
                res.Message = "Error";
                res.ErrorMessages = new List<string> { "productName field is not null or empty" };
                res.StatusCode = StatusCodes.Status400BadRequest.ToString();
                return BadRequest(res);
            }
            try
            {
                bool isSuccess = await _serv.updateProduct(id, updateProduct);
                if (isSuccess)
                {
                    res.Data = updateProduct;
                    res.Success = true;
                    res.Message = "Success";
                    res.StatusCode = StatusCodes.Status201Created.ToString();
                    return Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.Success = false;
                    res.Message = "Error";
                    res.ErrorMessages = new List<string> { "id exceed range of productid" };
                    res.StatusCode = StatusCodes.Status404NotFound.ToString();
                    return NotFound(res);
                }
                
            }
            catch
            {
                res.Data = null;
                res.Success = false;
                res.Message = "Error";
                res.ErrorMessages = new List<string> { "id exceed range of productid" };
                res.StatusCode = StatusCodes.Status404NotFound.ToString();
                return NotFound(res);
            }
            

        }


        [HttpDelete]
        [Route("Product/{id}")]
        public async Task<IActionResult> deleteProuductById(int id)
        {
            ServiceResponse<int> res = new ServiceResponse<int>();
            try
            {
                bool isSuccess = await _serv.deleteProduct(id);
                if (isSuccess)
                {
                    res.Data = id;
                    res.Success = true;
                    res.Message = "Success";
                    res.StatusCode = StatusCodes.Status201Created.ToString();
                    return Ok(res);
                }
                else
                {
                    res.Data = -1;
                    res.Success = false;
                    res.Message = "Error";
                    res.ErrorMessages = new List<string> { "id exceed range of productid" };
                    res.StatusCode = StatusCodes.Status404NotFound.ToString();
                    return NotFound(res);
                }
            }
            catch
            {
                res.Data = -1;
                res.Success = false;
                res.Message = "Error";
                res.ErrorMessages = new List<string> { "id exceed range of productid" };
                res.StatusCode = StatusCodes.Status404NotFound.ToString();
                return NotFound(res);
            }
        }
    }
}
