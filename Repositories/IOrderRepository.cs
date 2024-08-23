//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using ShopEaseApp.Models;
//using System.Linq;
//namespace ShopEaseApp.Models
//{
//    public interface IOrderRepository
//    {
//        Task<Order> GetOrderByIdAsync(int orderId);
//        Task<IEnumerable<Order>> GetAllOrdersAsync();
//        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
//        Task<bool> AddOrderAsync(Order order);
//        Task<bool> UpdateOrderAsync(Order order);
//        Task<bool> DeleteOrderAsync(int orderId);
//        Task<bool> ConfirmOrderAsync(int orderId);
//    }

//    public class OrderRepository : IOrderRepository
//    {
//        private readonly ShoppingDataContext.ShoppingModelDB _context;

//        public OrderRepository(ShoppingDataContext.ShoppingModelDB context)
//        {
//            _context = context;
//        }
//        public async Task<bool> AddOrderAsync(Order order)
//        {
//            await _context.Orders.AddAsync(order);
//            return await _context.SaveChangesAsync() > 0;
//        }

//        public async Task<bool> ConfirmOrderAsync(int orderId)
//        {
//            var order = await _context.Orders.FindAsync(orderId);
//            if (order != null)
//            {
//                order.OrderStatus = true;
//                return await _context.SaveChangesAsync() > 0;
//            }
//            return false;
//        }

//        public async Task<bool> DeleteOrderAsync(int orderId)
//        {
//            var order = await _context.Orders.FindAsync(orderId);
//            if (order != null)
//            {
//                _context.Orders.Remove(order);
//                return await _context.SaveChangesAsync() > 0;
//            }
//            return false;
//        }

//        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
//        {
//            return await _context.Orders
//                .Include(o => o.User)
//                .ToListAsync();
//        }

//        public async Task<Order> GetOrderByIdAsync(int orderId)
//        {
//            return await _context.Orders
//                .Include(o => o.User)
//                .FirstOrDefaultAsync(o => o.OrderID == orderId);
//        }

//        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
//        {
//            return await _context.Orders
//               .Where(o => o.User.UserID == userId)
//               .Include(o => o.User)
//               .ToListAsync();
//        }

//        public async Task<bool> UpdateOrderAsync(Order order)
//        {
//            _context.Orders.Update(order);
//            return await _context.SaveChangesAsync() > 0;
//        }
//    }
//}
