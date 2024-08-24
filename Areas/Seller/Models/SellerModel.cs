using ShopEaseApp.Models;
using System.Collections.Generic;
using static ShopEaseApp.Models.ShoppingDataContext;

namespace ShopEaseApp.Areas.Seller.Models
{
    public interface ISellerModel
    {
        public void Create(Product model,int? id);
        public bool Update(int productId, Product model, int? userId);

        public bool Delete(int productId, int? userId);
        List<Product> GetAllProducts();
        //public bool ConfirmOrder(int OrderID);
        //public bool ConfirmPayment(int ProductId);
        //List<OrderDetail> GetAllOrders();
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

        public void Create(Product model,int? id)
        {
            model.UserID = id;
           _dbContext.Products.Add(model);
            _dbContext.SaveChanges();
        }

        
            public bool Delete(int productId, int? userId)
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.ProductID == productId && p.UserID == userId);
                if (product == null)
                {
                    return false; 
                }

                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();

                return true;
            }
        

        //public List<OrderDetail> GetAllOrders()
        //{
        //    return _dbContext.OrderDetails.OrderBy(x => x.Order).ToList();
        //}

        public List<Product> GetAllProducts()
        {
              return _dbContext.Products.ToList();  
        }

        public bool Update(int productId, Product model, int? userId)
        {
            var existingProduct = _dbContext.Products.FirstOrDefault(p => p.ProductID == productId && p.UserID == userId);
            if (existingProduct == null)
            {
                return false; 
            }

           
            existingProduct.ProductName = model.ProductName;
            existingProduct.ProductDescription = model.ProductDescription;
            existingProduct.Price = model.Price;
            existingProduct.StockQuantity = model.StockQuantity;
            

            _dbContext.Products.Update(existingProduct);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
