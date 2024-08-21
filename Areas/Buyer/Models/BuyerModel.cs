using ShopEaseApp.Models;

namespace ShopEaseApp.Areas.Buyer.Models
{
    public interface BuyerModel
    {
        public Product Search(string name);
    }
    public class IBuyer : BuyerModel
    {
        ShoppingDataContext.ShoppingModelDB _datacontext;
        public IBuyer(ShoppingDataContext.ShoppingModelDB datacontext)
        {
            _datacontext = datacontext;
        }
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
