using src.Utils;
using static src.DTO.ProductDTO;

namespace src.Services.product
{
    public interface IProductService
    {
        Task<ProductReadDto> CreateOneAsync(ProductCreateDto CreateDto);
        Task<List<ProductReadDto>> GetAllAsync(PaginationOptions options);
        Task<ProductReadDto> GetByIdAsync(Guid id);
        Task<bool> DeleteOneAsync(Guid id);
        Task<bool> UpdateOneAsync(Guid id, ProductUpdateDto UpdateDto);
        // Task<List<ProductReadDto>> SearchProductsAsync(PaginationOptions searchOptions, PaginationOptions paginationOptions);
        // Task<List<ProductReadDto>> GetAllWithSortingAndFilteringAsync(PaginationOptions paginationOptions);

        //******************************************************************************
         Task<int> CountProductsAsync(); 
    }
}
