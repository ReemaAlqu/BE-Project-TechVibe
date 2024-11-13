
using src.Entity;
using static src.DTO.ProductDTO;

namespace src.DTO
{
    public class OrderDetailDTO
    {
        public class OrderDetailCreateDto
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
            // public  Product? Product {  get; internal set; } 

        }

        public class OrderDetailReadDto
        {
            public Guid Id { get; set; }
            public ProductReadDto Product { get; set; }
             public int Quantity { get; set; }

        }

     
    }
}
