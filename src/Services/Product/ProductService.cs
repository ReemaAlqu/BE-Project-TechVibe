using AutoMapper;
using src.Entity;
using src.Repository;
using src.Utils;
using static src.DTO.ProductDTO;

namespace src.Services.product
{
    public class ProductService : IProductService
    {
        protected readonly IProductRepository _productRepository;
        protected readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductReadDto> CreateOneAsync(ProductCreateDto createDto)
        {
            var product = _mapper.Map<ProductCreateDto, Product>(createDto);
            var productCreated = await _productRepository.CreateOneAsync(product);
            return _mapper.Map<Product, ProductReadDto>(productCreated);
        }

        public async Task<List<ProductReadDto>> GetAllAsync(PaginationOptions options)
        {
            var productList = await _productRepository.GetAllAsync(options);
            return _mapper.Map<List<Product>, List<ProductReadDto>>(productList);
        }

        // new => total amount of products
        public async Task<int> CountProductsAsync()
        {
            // return 12
            return await _productRepository.CountAsync();
        }

        public async Task<ProductReadDto> GetByIdAsync(Guid id)
        {
            var foundProduct = await _productRepository.GetByIdAsync(id);
            if (foundProduct == null)
            {
                throw CustomException.NotFound($"Product with ID '{id}' not found.");
            }

            return _mapper.Map<Product, ProductReadDto>(foundProduct);
        }

        public async Task<bool> DeleteOneAsync(Guid id)
        {
            var foundProduct = await _productRepository.GetByIdAsync(id);
            if (foundProduct != null)
            {
                bool isDeleted = await _productRepository.DeleteOneAsync(foundProduct);
                if (isDeleted)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> UpdateOneAsync(Guid id, ProductUpdateDto UpdateDto)
        {
            var foundProduct = await _productRepository.GetByIdAsync(id);
            if (foundProduct == null)
            {
                throw CustomException.NotFound($"Product with ID '{id}' not found.");
            }
            _mapper.Map(UpdateDto, foundProduct);
            return await _productRepository.UpdateOneAsync(foundProduct);
        }

        // public async Task<List<ProductReadDto>> SearchProductsAsync(PaginationOptions searchOptions, PaginationOptions paginationOptions)
        // {
        //     var products = await _productRepository.SearchProductsAsync(searchOptions, paginationOptions);
        //     return _mapper.Map<List<ProductReadDto>>(products);
        // }
        // public async Task<List<ProductReadDto>> GetAllWithSortingAndFilteringAsync(PaginationOptions paginationOptions)
        // {
        //     var products = await _productRepository.GetAllWithSortingAndFilteringAsync(paginationOptions);

        //     return _mapper.Map<List<ProductReadDto>>(products);
        // }
    }
}
