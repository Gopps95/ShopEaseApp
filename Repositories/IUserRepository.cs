using Microsoft.EntityFrameworkCore;
using ShopEaseApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopEaseApp.Repositories
{
    // Common interface for user operations
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<bool> RegisterUserAsync(User user); // Register a new user
        bool LoginAsync(string username, string password); // Login a user
        Task<bool> UpdateUserAsync(User user);
    }

    // Interface for seller operations






    public interface ISellerRepository : IUserRepository
    {
        Task<bool> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> RemoveProductAsync(int productId);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<bool> ConfirmOrderAsync(int orderId);
        Task<Order> GetOrderDetailsAsync(int orderId);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsForOrderAsync(int orderId);
        Task<bool> ConfirmPaymentAsync(int paymentId);
    }

    //    // Interface for buyer
    public interface IBuyerRepository : IUserRepository
    {
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<Product> GetProductByIdAsync(int productId);
        Task<bool> AddToCartAsync(int userId, int productId, int quantity);
        Task<bool> ConfirmOrderAsync(int userId);
        Task<bool> ConfirmPaymentAsync(int orderId, Payment payment);
        Task<Order> GetOrderDetailsAsync(int orderId);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsForOrderAsync(int orderId);
    }

    //    // Implementation of the seller repository
    public class SellerRepository : ISellerRepository
    {
        private readonly ShoppingDataContext.ShoppingModelDB _context;

        public SellerRepository(ShoppingDataContext.ShoppingModelDB context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.User.FindAsync(userId);
        }



        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.User.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<bool> ConfirmOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.OrderStatus = true;
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Order> GetOrderDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsForOrderAsync(int orderId)
        {
            return await _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.Order.OrderID == orderId)
                .ToListAsync();
        }

        public async Task<bool> ConfirmPaymentAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment != null)
            {
                payment.PaymentStatus = true;
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> RegisterUserAsync(User user)
        {
            await _context.User.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public bool LoginAsync(string username, string password)
        {
           bool status = false;
            User user = _context.User.Where(x => x.UserName == username).FirstOrDefault();
            if (user != null)
           {
                if (user.Password == password)
                {
                    status = true;
                }


           }

         return status;
        }
    }

    // Implementation of the buyer repository
    public class BuyerRepository : IBuyerRepository
    {
        private readonly ShoppingDataContext.ShoppingModelDB _context;

        public BuyerRepository(ShoppingDataContext.ShoppingModelDB context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.User.FindAsync(userId);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.User.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _context.Products
                .Where(p => p.ProductName.Contains(searchTerm) || p.ProductDescription.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<bool> AddToCartAsync(int userId, int productId, int quantity)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.User.UserID == userId && !o.OrderStatus);
            if (order == null)
            {
                order = new Order { User = await _context.User.FindAsync(userId) };
                await _context.Orders.AddAsync(order);
            }

            var orderDetail = new OrderDetail
            {
                Order = order,
                Product = await _context.Products.FindAsync(productId),
                Quantity = quantity
            };

            await _context.OrderDetails.AddAsync(orderDetail);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ConfirmOrderAsync(int userId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.User.UserID == userId && !o.OrderStatus);
            if (order != null)
            {
                order.OrderStatus = true;
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> ConfirmPaymentAsync(int orderId, Payment payment)
        {
            payment.Order = await _context.Orders.FindAsync(orderId);
            await _context.Payments.AddAsync(payment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Order> GetOrderDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsForOrderAsync(int orderId)
        {
            return await _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.Order.OrderID == orderId)
                .ToListAsync();
        }
        public async Task<bool> RegisterUserAsync(User userregistration)
        {
            await _context.User.AddAsync(userregistration);
            return await _context.SaveChangesAsync() > 0;
        }



        public bool LoginAsync(string username, string password)
        {

            bool status = false;
            User user = _context.User.Where(x => x.UserName == username).FirstOrDefault();
            if (user != null)
            {
                if (user.Password == password)
                {
                    status = true;
                }


            }

            return status;

        }
    }


}