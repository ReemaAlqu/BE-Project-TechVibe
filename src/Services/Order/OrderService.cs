using AutoMapper;
using src.DTO;
using src.Entity;
using src.Repository;

namespace src.Services
{
    public class OrderService : IOrderService
    {
        protected readonly IMapper _mapper;
        protected readonly IOrderRepository _ordersRepo;

        public OrderService(IMapper mapper, IOrderRepository order)
        {
            _mapper = mapper;
            _ordersRepo = order;
        }

        public async Task<IEnumerable<OrderDTO.Get>> GetAllOrdersAsync()
        {
            return (await _ordersRepo.GetAllOrdersAsync()).Select(order =>
                _mapper.Map<OrderDTO.Get>(order)
            );
        }

        public async Task<OrderDTO.Get> CreateOneOrderAsync(Guid userID, OrderDTO.Create orderDTO)
        {
            // Map DTO to Entity
            Order order = _mapper.Map<OrderDTO.Create, Order>(orderDTO);
            order.UserID = userID;

            // Add the order to the repository (this will calculate OriginalPrice dynamically)
            Order createdOrder = await _ordersRepo.AddOrderAsync(order);

            // Map the created order back to the DTO for response
            return _mapper.Map<Order, OrderDTO.Get>(createdOrder);
        }

        // public async Task<OrderDTO.Get?> GetOrderByIdAsync(Guid id)
        // {
        //     var requestedOrder = await _ordersRepo.GetOrderByIdAsync(id);
        //     return _mapper.Map<OrderDTO.Get>(requestedOrder);
        // }


        public async  Task<OrderDTO.Get> GetOrderByIdAsync(Guid orderId)
        {
            var requestedOrder = await _ordersRepo.GetOrderByIdAsync(orderId);
            return _mapper.Map<OrderDTO.Get>(requestedOrder);
        }

        public async Task<bool> UpdateOrderAsync(Guid id, OrderDTO.Update orderDTO)
        {
            var foundOrder = await _ordersRepo.GetOrderByIdAsync(id);
            if (foundOrder == null)
            {
                return false;
            }
            _mapper.Map(orderDTO, foundOrder);
            return await _ordersRepo.UpdateOrderAsync(foundOrder);
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            return await _ordersRepo.DeleteOrderAsync(id);
        }

        public  async Task<IEnumerable<OrderDTO.Get>> GetOrdersByUserId(Guid userId)
        {
            return (await _ordersRepo.GetOrdersByUserId(userId)).Select(order =>
                _mapper.Map<OrderDTO.Get>(order)
            );
        }
    }
}
