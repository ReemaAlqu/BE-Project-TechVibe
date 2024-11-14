using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Services.category;
using src.Utils;
using static src.DTO.CategoryDTO;

namespace src.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class CategoryController : ControllerBase
    {
        protected readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService service)
        {
            _categoryService = service;
        }

        // Get all categories with pagination
        [HttpGet]
        public async Task<ActionResult<List<CategoryReadDto>>> GetCategories(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync(paginationOptions);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError(ex.Message);
            }
        }

        // Get a category by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<List<CategoryReadDto>>> GetCategory([FromRoute] Guid id)
        {
            try
            {
                var category = await _categoryService.GetCategoryAsync(id);
                return Ok(category);
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError(ex.Message);
            }
        }

        // Create a new category
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CategoryReadDto>> CreateOne(
            [FromBody] CategoryCreateDto createDto
        )
        {
            try
            {
                var createdCategory = await _categoryService.CreateOneAsync(createDto);
                return Created($"/api/v1/Category/{createdCategory.Id}", createdCategory);
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError(ex.Message);
            }
        }

        // Delete a category
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> DeleteCategory([FromRoute] Guid id)
        {
            try
            {
                var deletedCategory = await _categoryService.DeleteOneAsync(id);
                if (!deletedCategory)
                {
                    throw CustomException.NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError(ex.Message);
            }
        }

        // Update a category
        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> UpdateCategory(
            [FromRoute] Guid id,
            [FromBody] CategoryUpdateDto updateDto
        )
        {
            try
            {
                var updateCategory = await _categoryService.UpdateOneAsync(id, updateDto);
                if (!updateCategory)
                {
                    throw CustomException.NotFound();
                }
                var updatedCategory = await _categoryService.GetCategoryAsync(id);
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError(ex.Message);
            }
        }
    }
}
