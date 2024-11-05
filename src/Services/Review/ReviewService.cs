using AutoMapper;
using src.Entity;
using src.Repository;
using src.Utils;
using static src.DTO.ReviewDTO;

namespace src.Services.review
{
    public class ReviewService : IReviewService
    {
        protected readonly ReviewRepository _reviewRepo;
        protected readonly IMapper _mapper;

        public ReviewService(ReviewRepository reviewRepo, IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
        }

        // Create a new review
        public async Task<ReviewReadDto> CreateOneAsync(ReviewCreateDto createDto, Guid userId)
        {
            if (createDto.Rating < 1 || createDto.Rating > 5)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(createDto.Rating),
                    "Rating must be between 1 and 5."
                );
            }

            var review = new Review
            {
                OrderId = createDto.OrderId,
                Comment = createDto.Comment,
                Rating = createDto.Rating,
                ReviewDate = DateTime.UtcNow,
                UserID = userId,
            };
            var reviewCreated = await _reviewRepo.CreateOneAsync(review);
            return _mapper.Map<Review, ReviewReadDto>(reviewCreated);
        }

        // Get all reviews
        public async Task<List<ReviewReadDto>> GetAllAsync()
        {
            try
            {
                var reviewList = await _reviewRepo.GetReviewsAsync();
                return _mapper.Map<List<Review>, List<ReviewReadDto>>(reviewList);
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError(ex.Message);
            }
        }

        // Get a review by ID
        public async Task<ReviewReadDto> GetByIdAsync(Guid id)
        {
            var foundReview = await _reviewRepo.GetReviewAsync(id);
            return _mapper.Map<Review?, ReviewReadDto>(foundReview);
        }

        // Delete a review
        public async Task<bool> DeleteOneAsync(Guid id)
        {
            var foundReview = await _reviewRepo.GetReviewAsync(id);
            if (foundReview != null)
            {
                bool isDeleted = await _reviewRepo.DeleteOneAsync(foundReview);

                if (isDeleted)
                {
                    return true;
                }
            }
            return false;
        }

        // Get reviews by order ID
        public async Task<List<ReviewReadDto>> GetReviewsByOrderIdAsync(Guid orderId)
        {
            var reviews = await _reviewRepo.GetReviewByOrderIdAsync(orderId);
            return _mapper.Map<List<Review>, List<ReviewReadDto>>(reviews);
        }

        // Update a review
        public async Task<bool> UpdateOneAsync(Guid id, ReviewUpdateDto updateDto)
        {
            var foundReview = await _reviewRepo.GetReviewAsync(id);

            if (foundReview == null)
            {
                return false;
            }
            _mapper.Map(updateDto, foundReview);
            return await _reviewRepo.UpdateOneAsync(foundReview);
        }
    }
}
