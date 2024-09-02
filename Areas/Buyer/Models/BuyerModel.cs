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

           

            int completedOrderCount = _datacontext.Orders.Count(o => o.UserID == userId && o.OrderStatus);

           

            int cycle = completedOrderCount / 5; 

            int orderInCycle = completedOrderCount % 5; 

           

            if (orderInCycle >= 0 && orderInCycle < 3)

            {

                return 0.10m; 

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

