using ShopEaseApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ShopEaseApp.Areas.Buyer.Models
{
    public interface BuyerModel
    {
        public List<Product> Search(string name);  // Updated to return a list of products
    }

    public class IBuyer : BuyerModel
    {
        private readonly ShoppingDataContext.ShoppingModelDB _datacontext;

        public IBuyer(ShoppingDataContext.ShoppingModelDB datacontext)
        {
            _datacontext = datacontext;
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
