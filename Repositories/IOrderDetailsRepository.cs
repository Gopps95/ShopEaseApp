//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using ShopEaseApp.Models;
//using System.Linq;

//namespace ShopEaseApp.Models
//{
//    public interface IOrderDetailsRepository
//    {

//        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
//        Task<OrderDetail> GetOrderDetailAsync(int orderId, int productId);
//        Task<bool> AddOrderDetailAsync(OrderDetail orderDetail);
//        Task<bool> UpdateOrderDetailAsync(OrderDetail orderDetail);
//        Task<bool> DeleteOrderDetailAsync(int orderId, int productId);
//    }

//    public class OrderDetailsRepository : IOrderDetailsRepository
//    {
//        private readonly ShoppingDataContext.ShoppingModelDB _context;

//        public OrderDetailsRepository(ShoppingDataContext.ShoppingModelDB context)
//        {
//            _context = context;
//        }
//        public async Task<bool> AddOrderDetailAsync(OrderDetail orderDetail)
//        {
//            await _context.OrderDetails.AddAsync(orderDetail);
//            return await _context.SaveChangesAsync() > 0;

//            //throw new NotImplementedException();
//        }

//        public async Task<bool> DeleteOrderDetailAsync(int orderId, int productId)
//        {
//            var orderDetail = await GetOrderDetailAsync(orderId, productId);
//            if (orderDetail != null)
//            {
//                _context.OrderDetails.Remove(orderDetail);
//                return await _context.SaveChangesAsync() > 0;
//            }
//            return false;
//            //throw new NotImplementedException();
//        }

//        public async Task<OrderDetail> GetOrderDetailAsync(int orderId, int productId)
//        {
//            return await _context.OrderDetails
//               .Include(od => od.Product)
//               .FirstOrDefaultAsync(od => od.Order.OrderID == orderId && od.Product.ProductID == productId);
//        }

//        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
//        {
//            return await _context.OrderDetails
//              .Include(od => od.Product)
//              .Where(od => od.Order.OrderID == orderId)
//              .ToListAsync();
//            //throw new NotImplementedException();
//        }

//        public async Task<bool> UpdateOrderDetailAsync(OrderDetail orderDetail)
//        {
//            _context.OrderDetails.Update(orderDetail);
//            return await _context.SaveChangesAsync() > 0;

//            // throw new NotImplementedException();
//        }
//    }
//}
