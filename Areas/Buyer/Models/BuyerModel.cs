using ShopEaseApp.Models;

using Microsoft.EntityFrameworkCore;

namespace ShopEaseApp.Areas.Buyer.Models

{

    public interface BuyerModel

    {

        public List<Product> Search(string name);  // Updated to return a list of products

        decimal CalculateDiscount(int userId);

    }

    public class IBuyer : BuyerModel

    {

        private readonly ShoppingDataContext.ShoppingModelDB _datacontext;

        public IBuyer(ShoppingDataContext.ShoppingModelDB datacontext)

        {

            _datacontext = datacontext;

        }

        public decimal CalculateDiscount(int userId)

        {

            // Fetch all completed orders for the user

            int completedOrderCount = _datacontext.Orders.Count(o => o.UserID == userId && o.OrderStatus);

            // Determine the cycle position

            int cycle = completedOrderCount / 5; // Every 5 orders starts a new discount cycle

            int orderInCycle = completedOrderCount % 5; // Determine order position within the cycle

            // Discount logic: After every 5 completed orders, next 3 orders get a discount

            if (orderInCycle >= 0 && orderInCycle < 3)

            {

                return 0.10m; // 10% discount for orders in positions 0, 1, and 2 within the cycle

            }

            else

            {

                return 0m; // No discount for other orders

            }

        }

        // Updated Search method to use EF.Functions.Like

        public List<Product> Search(string name)

        {

            // Use EF.Functions.Like for partial matching

            var matchedProducts = _datacontext.Products

                .Where(p => EF.Functions.Like(p.ProductName, $"%{name}%")) // Use wildcards for partial matches

                .ToList();

            return matchedProducts;  // Return the list of matched products

        }

    }

}

