using src.Utils;
using static src.DTO.ReviewDTO;

namespace src.Services.review
{
    public interface IReviewService
    {
        Task<ReviewReadDto> CreateOneAsync(ReviewCreateDto createDto, Guid userId);
        Task<List<ReviewReadDto>> GetAllAsync();
        Task<ReviewReadDto> GetByIdAsync(Guid id);
        Task<bool> DeleteOneAsync(Guid id);
        Task<bool> UpdateOneAsync(Guid id, ReviewUpdateDto updateDto);
        Task<List<ReviewReadDto>> GetReviewsByOrderIdAsync(Guid orderId);
    }
}
