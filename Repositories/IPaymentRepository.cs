//using Microsoft.EntityFrameworkCore;
//namespace ShopEaseApp.Models
//{
//    public interface IPaymentRepository
//    {
//        Task<Payment> GetPaymentByIdAsync(int paymentId);
//        Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId);
//        Task<bool> AddPaymentAsync(Payment payment);
//    }
//    public class PaymentRepository : IPaymentRepository
//    {
//        private readonly ShoppingDataContext.ShoppingModelDB _context;

//        public PaymentRepository(ShoppingDataContext.ShoppingModelDB context)
//        {
//            _context = context;
//        }

//        public async Task<bool> AddPaymentAsync(Payment payment)
//        {
//            await _context.Payments.AddAsync(payment);
//            return await _context.SaveChangesAsync() > 0;

//        }

//        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
//        {
//            return await _context.Payments
//                  .Include(p => p.Order)
//                  .FirstOrDefaultAsync(p => p.PaymentID == paymentId);
//        }

//        public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId)
//        {
//            return await _context.Payments
//               .Include(p => p.Order)
//               .Where(p => p.Order.OrderID == orderId)
//               .ToListAsync();
//        }
//    }
//}
