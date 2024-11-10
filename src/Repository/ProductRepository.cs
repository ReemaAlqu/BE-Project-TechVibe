using Microsoft.EntityFrameworkCore;
using src.Database;
using src.Entity;
using src.Utils;

namespace src.Repository
{
    public interface IProductRepository
    {
        Task<Product> CreateOneAsync(Product newProduct);

        Task<bool> DeleteOneAsync(Product product);
        Task<List<Product>> GetAllAsync(PaginationOptions options);
        Task<Product?> GetByIdAsync(Guid id);
        Task<bool> UpdateOneAsync(Product updateProduct);

        // Task<List<Product>> SearchProductsAsync(
        //     PaginationOptions searchOptions,
        //     PaginationOptions paginationOptions
        // );
        // Task<List<Product>> GetAllWithSortingAndFilteringAsync(PaginationOptions paginationOptions);


        //****************************************
        Task<int> CountAsync();
    }

    public class ProductRepository : IProductRepository
    {
        protected DbSet<Product> _product;
        protected DatabaseContext _databaseContext;

        // inject the DB (depedancy injection)
        public ProductRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _product = databaseContext.Set<Product>();
        }

        // Create a new product
        public async Task<Product> CreateOneAsync(Product newProduct)
        {
            await _product.AddAsync(newProduct);
            await _databaseContext.SaveChangesAsync();
            return newProduct;
        }

        // // Get all products
        // public async Task<List<Product>> GetAllAsync(PaginationOptions paginationOptions)
        // {
        //     return await _product.Include(p => p.Category).ToListAsync();
        // }

        // Get a product by Id
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            // .Include(p => p.Category.Name)
            return await _product.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        // Update a product
        public async Task<bool> UpdateOneAsync(Product updateProduct)
        {
            _product.Update(updateProduct);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        // Delete a product
        public async Task<bool> DeleteOneAsync(Product product)
        {
            _product.Remove(product);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        // public async Task<List<Product>> SearchProductsAsync(
        //     PaginationOptions searchOptions,
        //     PaginationOptions paginationOptions
        // )
        // {
        //     var query = _product.AsQueryable();

        //     if (!string.IsNullOrWhiteSpace(searchOptions.Name))
        //     {
        //         query = query.Where(p => p.Name.ToLower().Contains(searchOptions.Name.ToLower()));
        //     }

        //     if (!string.IsNullOrWhiteSpace(searchOptions.Description))
        //     {
        //         query = query.Where(p =>
        //             p.Description.ToLower().Contains(searchOptions.Description.ToLower())
        //         );
        //     }

        //     var products = await query
        //         .Include(p => p.Category)
        //         .Skip(paginationOptions.Offset)
        //         .Take(paginationOptions.Limit)
        //         .ToListAsync();

        //     return products;
        // }

        // public async Task<List<Product>> GetAllWithSortingAndFilteringAsync(
        //     PaginationOptions paginationOptions
        // )
        // {
        //     var query = _product.AsQueryable();

        //     if (paginationOptions.Filter.MinPrice.HasValue)
        //     {
        //         query = query.Where(p => p.Price >= paginationOptions.Filter.MinPrice.Value);
        //     }

        //     if (paginationOptions.Filter.MaxPrice.HasValue)
        //     {
        //         query = query.Where(p => p.Price <= paginationOptions.Filter.MaxPrice.Value);
        //     }

        //     if (!string.IsNullOrWhiteSpace(paginationOptions.Filter.Category))
        //     {
        //         var categoryFilter = paginationOptions.Filter.Category.ToLower();
        //         query = query.Where(p => p.Category.Name.ToLower().Contains(categoryFilter));
        //     }

        //     query = paginationOptions.Sort.SortBy.ToLower() switch
        //     {
        //         "price" => (paginationOptions.Sort.SortDescending ?? false)
        //             ? query.OrderByDescending(p => p.Price)
        //             : query.OrderBy(p => p.Price),

        //         "name" => (paginationOptions.Sort.SortDescending ?? false)
        //             ? query.OrderByDescending(p => p.Name)
        //             : query.OrderBy(p => p.Name),

        //         _ => query.OrderBy(p => p.Name),
        //     };

        //     var totalItems = await query.CountAsync();

        //     var products = await query
        //         .Include(p => p.Category)
        //         .Skip(paginationOptions.Offset)
        //         .Take(paginationOptions.Limit)
        //         .AsNoTracking()
        //         .ToListAsync();

        //     return products;
        // }

        //*********************************************************************************************************




        // Get all products with  pagination
        public async Task<List<Product>> GetAllAsync(PaginationOptions options)
        {
            // Start with all products
            var products = _product.Include(p => p.Category).ToList();

            // search
            if (!string.IsNullOrEmpty(options.Search))
            {
                products = products
                    .Where(p => p.Name.Contains(options.Search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // min price
            if (options.MinPrice.HasValue && options.MinPrice > 0)
            {
                products = products.Where(p => p.Price >= options.MinPrice).ToList();
            }
            // max price
            if (options.MinPrice.HasValue && options.MaxPrice < decimal.MaxValue)
            {
                products = products.Where(p => p.Price <= options.MaxPrice).ToList();
            }

            // Apply pagination
            products = products.Skip(options.Offset).Take(options.Limit).ToList();

            return products;
        }

        // Count all products
        public async Task<int> CountAsync()
        {
            return await _databaseContext.Set<Product>().CountAsync();
        }

        // Partial update (PATCH)
        public async Task<Product?> PatchOneAsync(Guid id, Product updatedFields)
        {
            var existingProduct = await GetByIdAsync(id);

            if (existingProduct == null)
            {
                return null; // Product not found
            }

            // Update only the fields that are not null
            if (!string.IsNullOrEmpty(updatedFields.Name))
            {
                existingProduct.Name = updatedFields.Name;
            }

            // Add other fields as needed, like price, description, etc.
            // if (updatedFields.Price != null) { existingProduct.Price = updatedFields.Price; }

            // Save the changes
            await _databaseContext.SaveChangesAsync();

            return existingProduct;
        }
    }
}
