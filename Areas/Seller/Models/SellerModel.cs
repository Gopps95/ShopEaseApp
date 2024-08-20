using ShopEaseApp.Models;
using System.Collections.Generic;
using static ShopEaseApp.Models.ShoppingModel;
using static ShopEaseApp.Models.ShoppingModelDB;


namespace ShopEaseApp.Areas.Seller.Models
{
    public interface SellerModel
    {
        public void Create(ShoppingModel.Product model);
        public void Update(ShoppingModel.Product model);
        public void Delete(ShoppingModel.Product model);
        List<ShoppingModel.Product> GetAllProducts();
        public bool ConfirmOrder(ShoppingModel.Product product);
        public bool ConfirmPayment(int ProductId);
        List<ShoppingModel.OrderDetail> GetAllOrders();
    }
    public class SellerModel1 : SellerModel
    {
        ShoppingModelDB _dbContext;
        public SellerModel1(ShoppingModelDB context)
        {
            _dbContext = context;
        }
       
        

        public bool ConfirmOrder(ShoppingModel.Product product)
        {
            foreach (var i in _dbContext.Products)
            {
                if (i.ProductID == product.ProductID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ConfirmPayment(int products)
        {
            foreach (var i in _dbContext.Products)
            {
                if (i.ProductID == products)
                {
                    return true;
                }
            }
            return false;
        }

        public void Create(ShoppingModel.Product model)
        {
            _dbContext.Products.Add(model);

            _dbContext.SaveChanges();
        }

        public void Delete(ShoppingModel.Product model)
        {
            _dbContext.Products.Remove(model);

            _dbContext.SaveChanges();
        }

        public List<ShoppingModel.OrderDetail> GetAllOrders()
        {
            return _dbContext.OrderDetails.OrderBy(x => x.Order).ToList();
        }

        public List<ShoppingModel.Product> GetAllProducts()
        {
            return _dbContext.Products.OrderBy(x => x.Products).ToList();
        }

        public void Update(ShoppingModel.Product model)
        {
            _dbContext.Products.Update(model);

            _dbContext.SaveChanges();
        }
    }
}
