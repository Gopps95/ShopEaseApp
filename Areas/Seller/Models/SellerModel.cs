using ShopEaseApp.Models;
using System.Collections.Generic;
using static ShopEaseApp.Models.ShoppingDataContext;

namespace ShopEaseApp.Areas.Seller.Models
{
    public interface ISellerModel
    {
        public void Create(Product model);
        //public Product Update(Product model);
        //public Product Delete(int ProductId);
        List<Product> GetAllProducts();
        //public bool ConfirmOrder(int OrderID);
        //public bool ConfirmPayment(int ProductId);
        List<OrderDetail> GetAllOrders();
    }
    public class SellerModel : ISellerModel
    {
        ShoppingModelDB _dbContext;
        public SellerModel(ShoppingModelDB context)
        {
            _dbContext = context;
        }
        //public bool ConfirmOrder(int OrderID)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool ConfirmPayment(int ProductId)
        //{
        //    throw new NotImplementedException();
        //}

        public void Create(Product model)
        {
           _dbContext.Products.Add(model);
            _dbContext.SaveChanges();
        }

        //public Product Delete(int ProductId)
        //{
        //    throw new NotImplementedException();
        //}

        public List<OrderDetail> GetAllOrders()
        {
            return _dbContext.OrderDetails.OrderBy(x => x.Order).ToList();
        }

        public List<Product> GetAllProducts()
        {
              return _dbContext.Products.ToList();  
        }

        //public Product Update(Product model)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
