using ShopEaseApp.Models;

namespace ShopEaseApp.Areas.Buyer.Models
{
    public interface BuyerModel
    {
        public Product Search(string name);
        //public void AddToCart(int productId, int quantity, int? userId);
        //public void RemoveFromCart(int productId, int? userId);

    }
    public class IBuyer : BuyerModel
    {
        ShoppingDataContext.ShoppingModelDB _datacontext;
        public IBuyer(ShoppingDataContext.ShoppingModelDB datacontext)
        {
            _datacontext = datacontext;
        }

        //public void AddToCart(int productId, int quantity, int? userId)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RemoveFromCart(int productId, int? userId)
        //{
        //    throw new NotImplementedException();
        //}

        public Product Search(string name)
        {
            Product model = new Product();
            foreach (var item in _datacontext.Products)
            {
                if (item.ProductName == name)
                {
                    model = item;
                }
            }
            return model;
            // throw new NotImplementedException();
        }
    }
}
