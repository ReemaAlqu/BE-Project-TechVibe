using Microsoft.EntityFrameworkCore;
using src.Database;
using src.Entity;

namespace src.Repository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> AddOrderAsync(Order newOrder);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(Guid id);
        Task<Order?> GetOrderByIdAsync(Guid id);
    }

    public class OrderRepository : IOrderRepository
    {
        protected readonly DatabaseContext _db;
        private DbSet<Order> _orders => _db.Order;

        public OrderRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id) =>
            await _orders.Include(order => order.User).FirstOrDefaultAsync(o => o.ID == id);

        public async Task<List<Order>> GetAllOrdersAsync() =>
            await _orders.Include(order => order.User).ToListAsync();

        // public async Task<Order> AddOrderAsync(Order newOrder)
        // {
        //     var result = await _orders.AddAsync(newOrder);
        //     await _db.SaveChangesAsync();
        //     return result.Entity;
        // }
        public async Task<Order> AddOrderAsync(Order newOrder)
        {
            // Add the new order to the repository
            await _orders.AddAsync(newOrder);
            await _db.SaveChangesAsync();

            // Load related entities in one go for better performance.
            await _db.Entry(newOrder)
                .Collection(o => o.OrderDetails)
                .Query()
                .Include(od => od.Product) // Eager load the related Product entity for each OrderDetail
                .LoadAsync();

            return newOrder;
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _orders.Update(order);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var existingOrder = await _orders.FirstOrDefaultAsync(o => o.ID == id);
            if (existingOrder == null)
            {
                return false;
            }
            _orders.Remove(existingOrder);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
