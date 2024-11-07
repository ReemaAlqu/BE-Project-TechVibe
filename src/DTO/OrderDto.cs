using src.Entity;
using static src.DTO.OrderDetailDTO;

namespace src.DTO
{
    public class OrderDTO
    {
        public class Update
        {
            public ICollection<OrderDetailCreateDto> OrderDetails { get; set; }
        }

        public class Create
        {
            public Guid UserID { get; set; }

            public List<OrderDetailCreateDto> OrderDetails { get; set; }
        }

        public class Get
        {
            public Guid ID { get; set; }
            public Guid UserID { get; set; }

            public IEnumerable<OrderDetailReadDto> OrderDetails { get; set; }
        }
    }
}
