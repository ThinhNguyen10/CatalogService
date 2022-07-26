using ApiEcommerce.DbCommerce;
using ApiEcommerce.Dtos.Category;
using ApiEcommerce.Dtos.Product;
using ApiEcommerce.Models;
using ApiEcommerce.Repository.Contract;
using ApiEcommerce.Service.Contract;
using ApiEcommerce.ServiceResponder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiEcommerce.Controllers
{
  
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _serv;
        private readonly ICategoryRepository _repo;
        public CategoryController(ICategoryService serv, ICategoryRepository repo)
        {
            _serv = serv;
            _repo = repo;
        }

        [HttpGet]
        [Route("Category/All")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDto>))]
        public async Task<IActionResult> GetAll()
        {
            ServiceResponse<List<CategoryDto>> res = new ServiceResponse<List<CategoryDto>>();

            try
            {
                var categories = await _serv.getAllCategory();

                res.Data = categories;
                res.Success = true;
                res.StatusCode = StatusCodes.Status200OK.ToString();
                res.Message = "Success";
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.Success = false;
                res.StatusCode = StatusCodes.Status404NotFound.ToString();
                res.Message = "Error";
                res.ErrorMessages.Add(ex.Message);
                return NotFound(res);
            }
           
            

            return Ok(res);
        }

        [HttpGet]
        [Route("Category/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDto>))]
        public async Task<IActionResult> getCategoryById(int id)
        {
            ServiceResponse<CategoryDto> res = new ServiceResponse<CategoryDto>();
            bool isExists = await _repo.categoryIsExists(id);
            if (!isExists)
            {
                res.Data = null;
                res.Message = "Error";
                res.Success = false;
                res.ErrorMessages = new List<string>() { "Id exceed range of categoryId"};
                res.StatusCode = StatusCodes.Status400BadRequest.ToString();
                return BadRequest(res);
            }
            var category = await _serv.getCategoryById(id);
            res.Data = category;
            res.Message = "Success";
            res.Success = true;
            res.StatusCode = StatusCodes.Status200OK.ToString();

            return Ok(res);
        }


        [HttpGet]
        [Route("Category/{categoryId}/Products")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
        public async Task<IActionResult> getProductListByCategoryId(int categoryId)
        {

            ServiceResponse<List<ProductDto>> res = new ServiceResponse<List<ProductDto>>();
            bool isExists = await _repo.categoryIsExists(categoryId);
            if (!isExists)
            {
                res.Data = null;
                res.Message = "Error";
                res.Success = false;
                res.ErrorMessages = new List<string>() { "Id exceed range of categoryId" };
                res.StatusCode = StatusCodes.Status400BadRequest.ToString();
                return BadRequest(res);
            }
            var productsDto = await _serv.getProductListByCategoryId(categoryId);
            res.Data = productsDto;
            res.Message = "Success";
            res.Success = true;
            res.StatusCode = StatusCodes.Status200OK.ToString();

            return Ok(res);
        }


        [HttpPost]
        [Route("Category")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
        public async Task<IActionResult> addNewCategory([FromBody] CreateCategoryDto createCategory)
        {
            ServiceResponse<CreateCategoryDto> res = new ServiceResponse<CreateCategoryDto>();
            string categoryName = createCategory.Name;
            string categorySlug = createCategory.Slug;
            string categoryDescription = createCategory.Description;
            if(String.IsNullOrEmpty(categoryName) || String.IsNullOrEmpty(categoryDescription) || String.IsNullOrEmpty(categorySlug))
            {
                res.Data = null;
                res.Message = "Error";
                res.Success = false;
                res.ErrorMessages = new List<string>() { "Parameter is not null or empty" };
                res.StatusCode = StatusCodes.Status400BadRequest.ToString();
                return BadRequest(res);
            }

            bool isSuccess = await _serv.addNewCategory(createCategory);
            if (isSuccess)
            {
                res.Data = createCategory;
                res.Message = "Success";
                res.Success = true;
                res.StatusCode = StatusCodes.Status201Created.ToString();
                return Created("https://localhost:7022/Category", res);
            }
            else
            {
                return BadRequest(res);
            }
           
        }


        [HttpPut]
        [Route("Category/{categoryId}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
        public async Task<IActionResult> updateCategory(int categoryId,[FromBody] UpdateCategoryDto updateCategory)
        {
            ServiceResponse<UpdateCategoryDto> res = new ServiceResponse<UpdateCategoryDto>();

            bool isExists = await _repo.categoryIsExists(categoryId);
            if (!isExists)
            {
                res.Data = null;
                res.Message = "Error";
                res.Success = false;
                res.ErrorMessages = new List<string>() { "Id exceed range of categoryId" };
                res.StatusCode = StatusCodes.Status400BadRequest.ToString();
                return BadRequest(res);
            }

            string categoryName = updateCategory.Name;
            string categorySlug = updateCategory.Slug;
            string categoryDescription = updateCategory.Description;
            if (String.IsNullOrEmpty(categoryName) || String.IsNullOrEmpty(categoryDescription) || String.IsNullOrEmpty(categorySlug))
            {
                res.Data = null;
                res.Message = "Error";
                res.Success = false;
                res.ErrorMessages = new List<string>() { "Parameter is not null or empty" };
                res.StatusCode = StatusCodes.Status400BadRequest.ToString();
                return BadRequest(res);
            }

            bool isSuccess = await _serv.updateCategory(categoryId,updateCategory);
            if (isSuccess)
            {
                res.Data = updateCategory;
                res.Message = "Success";
                res.Success = true;
                res.StatusCode = StatusCodes.Status200OK.ToString();
                return Ok(res);
            }
            else
            {
                return NotFound(res);
            }

        }

        [HttpDelete]
        [Route("Category/{categoryId}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
        public async Task<IActionResult> deleteCategory(int categoryId)
        {
            ServiceResponse<int> res = new ServiceResponse<int>();

            bool isExists = await _repo.categoryIsExists(categoryId);
            if (!isExists)
            {
                res.Data = -1;
                res.Message = "Error";
                res.Success = false;
                res.ErrorMessages = new List<string>() { "Id exceed range of categoryId" };
                res.StatusCode = StatusCodes.Status400BadRequest.ToString();
                return BadRequest(res);
            }

            bool isSuccess = await _serv.deleteCategory(categoryId);
            if (isSuccess)
            {
                res.Data = categoryId;
                res.Message = "Success";
                res.Success = true;
                res.StatusCode = StatusCodes.Status200OK.ToString();
                return Ok(res);
            }
            else
            {
                return NotFound(res);
            }

        }
       
    }
}
