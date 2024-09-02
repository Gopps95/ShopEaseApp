using ShopEaseApp.Models;

using System.Collections.Generic;

using System.Linq;  // For LINQ queries

using Microsoft.AspNetCore.Http;  // For accessing session

using static ShopEaseApp.Models.ShoppingDataContext;

namespace ShopEaseApp.Areas.Seller.Models

{

    public interface ISellerModel

    {

        void Create(Product model, int? id);

        bool Update(int productId, Product model, int? userId);

        bool Delete(int productId, int? userId);

        List<Product> GetAllProducts();

        List<Product> GetProductsBoughtFromSeller(int? userId);  // Method to get products bought from the seller

        (int SellerId, string SellerName) GetSellerInfo(int? userId, IHttpContextAccessor httpContextAccessor);  // Method to get seller information

    }

    public class SellerModel : ISellerModel

    {

        private readonly ShoppingModelDB _dbContext;

        private readonly IHttpContextAccessor _httpContextAccessor;  // Accessing the HttpContext

        public SellerModel(ShoppingModelDB context, IHttpContextAccessor httpContextAccessor)

        {

            _dbContext = context;

            _httpContextAccessor = httpContextAccessor;

        }

        public void Create(Product model, int? id)

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

        public List<Product> GetProductsBoughtFromSeller(int? userId)

        {

            if (userId == null) return new List<Product>();

            var productsBought = (from orderDetail in _dbContext.OrderDetails

                                  join product in _dbContext.Products on orderDetail.ProductID equals product.ProductID

                                  where product.UserID == userId

                                  select product).ToList();

            return productsBought;

        }

        // Updated method to get seller information and fetch username from session

        public (int SellerId, string SellerName) GetSellerInfo(int? userId, IHttpContextAccessor httpContextAccessor)

        {

            if (userId == null) return (0, string.Empty);

            // Retrieve Username from session

            string username = httpContextAccessor.HttpContext.Session.GetString("Username");

            var seller = _dbContext.User

                .Where(u => u.UserID == userId)

                .Select(u => new { SellerId = u.UserID, SellerName = username })  // Fetching Username from session

                .FirstOrDefault();

            return seller != null ? (seller.SellerId, seller.SellerName) : (0, string.Empty);

        }

    }

}

