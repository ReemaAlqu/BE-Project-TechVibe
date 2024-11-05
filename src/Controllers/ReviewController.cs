using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Services.review;
using src.Utils;
using static src.DTO.ReviewDTO;

namespace src.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        protected readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET api/v1/review
        [HttpGet]
        public async Task<ActionResult<List<ReviewReadDto>>> GetAll()
        {
            try
            {
                var reviews = await _reviewService.GetAllAsync();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError(ex.Message);
            }
        }

        // Get a review by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewReadDto>> GetById([FromRoute] Guid id)
        {
            try
            {
                var review = await _reviewService.GetByIdAsync(id);
                if (review == null)
                {
                    throw CustomException.NotFound();
                }
                return Ok(review);
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError(ex.Message);
            }
        }

        // Get reviews by order ID
        [HttpGet("/order/{orderId}")]
        public async Task<ActionResult<List<ReviewReadDto>>> GetReviewsByOrderId(
            [FromRoute] Guid orderId
        )
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByOrderIdAsync(orderId);
                if (reviews == null || !reviews.Any())
                {
                    CustomException.NotFound();
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError(ex.Message);
            }
        }

        // Create a new review
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReviewReadDto>> CreateReview(
            [FromBody] ReviewCreateDto createDto
        )
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(c =>
                    c.Type == ClaimTypes.NameIdentifier
                );
                if (userIdClaim == null)
                {
                    throw CustomException.UnAuthorized();
                }
                var userGuid = Guid.Parse(userIdClaim.Value);
                var reivewCreated = await _reviewService.CreateOneAsync(createDto, userGuid);
                return Created($"api/v1/review/{reivewCreated.ReviewId}", reivewCreated);
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError(ex.Message);
            }
        }

        // Delete a review
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteReview(Guid id)
        {
            try
            {
                var isDeleted = await _reviewService.DeleteOneAsync(id);
                if (!isDeleted)
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

        // Update a review
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ReviewReadDto>> UpdateReview(
            Guid id,
            [FromBody] ReviewUpdateDto updateDto
        )
        {
            try
            {
                var isUpdated = await _reviewService.UpdateOneAsync(id, updateDto);
                if (!isUpdated)
                {
                    throw CustomException.NotFound();
                }
                var updatedReview = await _reviewService.UpdateOneAsync(id, updateDto);
                return Ok(updatedReview);
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError(ex.Message);
            }
        }
    }
}
