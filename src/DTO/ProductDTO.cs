using src.Entity;

namespace src.DTO
{
    public class ProductDTO
    {
        public class ProductCreateDto
        {
            public required string Name { get; set; }
            public required decimal Price { get; set; }
            public required string Description { get; set; }
            public required string ImageUrl { get; set; }
            public Guid CategoryId { get; set; }
        }

        public class ProductListDto
        {
            public List<ProductReadDto> Products { get; set; }
            public int TotalCount { get; set; }
        }

        public class ProductReadDto
        {
            public Guid Id { get; set; }
            public required string Name { get; set; }
            public required decimal Price { get; set; }
            public required string ImageUrl { get; set; }
            public required string Description { get; set; }
            public Guid CategoryId { get; set; }
            public required Category Category { get; set; }
        }

        public class ProductUpdateDto
        {
            public  string? Name { get; set; }
            public decimal? Price { get; set; }
            public string? Description { get; set; }
        }
    }
}
