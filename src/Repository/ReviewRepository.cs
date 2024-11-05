using Microsoft.EntityFrameworkCore;
using src.Database;
using src.Entity;
using src.Utils;

namespace src.Repository
{
    public class ReviewRepository
    {
        protected DbSet<Review> _review;
        protected DatabaseContext _databaseContext;

        public ReviewRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _review = databaseContext.Set<Review>();
        }

        // Create a new review
        public async Task<Review> CreateOneAsync(Review newReview)
        {
            try
            {
                await _review.AddAsync(newReview);
                await _databaseContext.SaveChangesAsync();
                return newReview;
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError("Failed to create review: " + ex.Message);
            }
        }

        // Get a review by ID
        public async Task<Review?> GetReviewAsync(Guid id)
        {
            try
            {
                return await _review
                    .Include(i => i.Order)
                    .FirstOrDefaultAsync(i => i.ReviewId == id);
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError("Error fetching review: " + ex.Message);
            }
        }

        // Get all reviews with pagination
        public async Task<List<Review>> GetReviewsAsync()
        {
            try
            {
                // var result = _review
                //     .Include(r => r.Order)
                //     .Where(r => r.Comment.ToLower().Contains(paginationOptions.Name));
                // return await result
                //     .
                // .ToListAsync();
                return await _review.ToListAsync();
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError("Error fetching reviews: " + ex.Message);
            }
        }

        // Get reviews by order ID
        public async Task<List<Review>> GetReviewByOrderIdAsync(Guid orderId)
        {
            try
            {
                return await _review
                    .Where(r => r.OrderId == orderId)
                    .Include(r => r.Order)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError(
                    "Error fetching reviews by order ID: " + ex.Message
                );
            }
        }

        // Delete a review
        public async Task<bool> DeleteOneAsync(Review review)
        {
            try
            {
                _review.Remove(review);
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError("Error deleting review: " + ex.Message);
            }
        }

        // Update a review
        public async Task<bool> UpdateOneAsync(Review updatedReview)
        {
            try
            {
                _review.Update(updatedReview);
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw CustomException.InternalError("Error updating review: " + ex.Message);
            }
        }
    }
}
