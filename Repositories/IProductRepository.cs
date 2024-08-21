//using Microsoft.EntityFrameworkCore;

//namespace ShopEaseApp.Models
//{
//    public interface IProductRepository
//    {
//        public interface IProductRepository
//        {
//            Task<IEnumerable<Product>> GetAllProductsAsync();
//            Task<Product> GetProductByIdAsync(int productId);
//            Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
//            Task<bool> AddProductAsync(Product product);
//            Task<bool> UpdateProductAsync(Product product);
//            Task<bool> DeleteProductAsync(int productId);
//            //Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
//            Task<bool> UpdateStockQuantityAsync(int productId, int newQuantity);
//        }

//        public class ProductRepository : IProductRepository
//        {
//            private readonly ShoppingDataContext.ShoppingModelDB _context;

//            public ProductRepository(ShoppingDataContext.ShoppingModelDB context)
//            {
//                _context = context;
//            }

//            public async Task<IEnumerable<Product>> GetAllProductsAsync()
//            {

//                return await _context.Products.ToListAsync();
//            }

//            public async Task<Product> GetProductByIdAsync(int productId)
//            {
//                return await _context.Products.FindAsync(productId);
//            }

//            public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
//            {
//                return await _context.Products
//                    .Where(p => p.ProductName.Contains(searchTerm) || p.ProductDescription.Contains(searchTerm))
//                    .ToListAsync();
//            }

//            public async Task<bool> AddProductAsync(Product product)
//            {
//                await _context.Products.AddAsync(product);
//                return await _context.SaveChangesAsync() > 0;
//            }

//            public async Task<bool> UpdateProductAsync(Product product)
//            {
//                _context.Products.Update(product);
//                return await _context.SaveChangesAsync() > 0;
//            }

//            public async Task<bool> DeleteProductAsync(int productId)
//            {
//                var product = await _context.Products.FindAsync(productId);
//                if (product != null)
//                {
//                    _context.Products.Remove(product);
//                    return await _context.SaveChangesAsync() > 0;
//                }
//                return false;
//            }

//            //public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
//            //{
//            //    return await _context.Products
//            //        .Where(p => p.ProductCategories.Any(pc => pc.CategoryID == categoryId))
//            //        .ToListAsync();
//            //}

//            public async Task<bool> UpdateStockQuantityAsync(int productId, int newQuantity)
//            {
//                var product = await _context.Products.FindAsync(productId);
//                if (product != null)
//                {
//                    product.StockQuantity = newQuantity;
//                    return await _context.SaveChangesAsync() > 0;
//                }
//                return false;
//            }
//        }
//    }
//}
