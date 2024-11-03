using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTO;
using src.Entity;
using src.Services.product;
using src.Utils;
using static src.DTO.ProductDTO;

namespace src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        protected IProductService _productService;

        public ProductsController(IProductService service)
        {
            _productService = service;
        }

        [HttpPost]
        // [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ProductReadDto>> CreateOne(
            [FromBody] ProductCreateDto createDto
        )
        {
            var productCreated = await _productService.CreateOneAsync(createDto);
            return Created($"api/v1/products/{productCreated.Id}", productCreated);
        }

        // [HttpGet]
        // public async Task<ActionResult<List<ProductReadDto>>> GetAll(
        //     [FromQuery] PaginationOptions paginationOptions
        // )
        // {
        //     var productList = await _productService.GetAllAsync(paginationOptions);
        //     return Ok(productList);
        // }

             [HttpGet]
        // old"Task<ActionResult<List<ProductReadDto>>>
        public async Task<ActionResult<List<ProductListDto>>> GetAllAsync([FromQuery] PaginationOptions options)
        {
            var productList = await _productService.GetAllAsync(options);
            // calculate the total count 
            var totalCount = await _productService.CountProductsAsync();

            var response = new ProductListDto
            {
                Products = productList,
                TotalCount = totalCount
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReadDto>> GetById([FromRoute] Guid id)
        {

            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] ProductUpdateDto updateDto
        )
        {
            var result = await _productService.UpdateOneAsync(id, updateDto);
            if (!result)
            {
                throw CustomException.NotFound($"Product with ID {id} not found.");
            }
            var updatedProduct = await _productService.GetByIdAsync(id);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _productService.DeleteOneAsync(id);
            if (!deleted)
            {
                throw CustomException.NotFound($"Product with ID {id} not found.");
            }
            return NoContent();
        }

        [HttpGet("search")]
        //search?filter.category&name=vitamin&limit=10&offset=0
        public async Task<ActionResult<List<ProductReadDto>>> SearchProducts(
       [FromQuery] PaginationOptions searchOptions,
       [FromQuery] PaginationOptions paginationOptions)
        {
            var products = await _productService.SearchProductsAsync(searchOptions, paginationOptions);

            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "No products found matching the search criteria." });
            }

            return Ok(products);
        }
        //api/v1/products/sorted-filtered?filter.category=Vitamins&limit=10&offset=0
        [HttpGet("sorted-filtered")]
        public async Task<ActionResult<List<ProductReadDto>>> GetAllWithSortingAndFiltering(
    [FromQuery] PaginationOptions paginationOptions)
        {
            var products = await _productService.GetAllWithSortingAndFilteringAsync(paginationOptions);

            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "No products found." });
            }

            return Ok(products);
        }

    }
}
