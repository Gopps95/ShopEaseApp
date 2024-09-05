using ShopEaseApp.Models;

using Microsoft.EntityFrameworkCore;

namespace ShopEaseApp.Areas.Buyer.Models

{

    public interface BuyerModel

    {

        public List<Product> Search(string name);  

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

            

            decimal totalAmountSpent = _datacontext.Orders

                .Where(o => o.UserID == userId && o.OrderStatus)

                .Sum(o => o.TotalAmount);

            

            if (totalAmountSpent > 50000)

            {

                return 0.10m; 

            }

            else if (totalAmountSpent > 10000)

            {

                return 0.07m; 

            }

            else if (totalAmountSpent > 3000)

            {

                return 0.05m; 

            }

            else

            {

                return 0.0m; 

            }

        }




        public List<Product> Search(string name)

        {

           

            var matchedProducts = _datacontext.Products

                .Where(p => EF.Functions.Like(p.ProductName, $"%{name}%")) 

                .ToList();

            return matchedProducts;  
        }

    }

}

