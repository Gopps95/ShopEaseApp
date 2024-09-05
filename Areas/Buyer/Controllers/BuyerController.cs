using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopEaseApp.Areas.Buyer.Models;
using ShopEaseApp.Models;
using static ShopEaseApp.Models.ShoppingDataContext;

namespace ShopEaseApp.Areas.Buyer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Buyer")]
    public class BuyerController : ControllerBase
    {
        BuyerModel _b;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ShoppingModelDB _dbContext;
        public BuyerController(BuyerModel b, IHttpContextAccessor httpContextAccessor, ShoppingModelDB dbContext)
        {
            _b=b;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }
        

        [HttpGet("DisplayCart")]
        public List<CartItems> Get()
        {
            var cartItems = _httpContextAccessor.HttpContext.Session.GetCart().MyCartItems;
            return cartItems;
        }


        
        [HttpGet("Search")]
        public List<Product> Get(string name)
        {
            return _b.Search(name);
        }
        [HttpPut("UpdateUserDetails")]

        public IActionResult UpdateUserDetails([FromBody] User updatedUserDetails)

        {

           

            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");

            if (userId == null)

            {

                return Unauthorized("User not logged in");

            }

            

            var existingUser = _dbContext.User.FirstOrDefault(u => u.UserID == userId);

            if (existingUser == null)

            {

                return NotFound("User not found");

            }

            

            if (!string.IsNullOrEmpty(updatedUserDetails.UserName))

                existingUser.UserName = updatedUserDetails.UserName;

            if (!string.IsNullOrEmpty(updatedUserDetails.EmailID))

                existingUser.EmailID = updatedUserDetails.EmailID;

            

            if (!string.IsNullOrEmpty(updatedUserDetails.Password))

            {

                

                string salt = BCrypt.Net.BCrypt.GenerateSalt();

                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(updatedUserDetails.Password, salt);

            }

            

            _dbContext.User.Update(existingUser);

            _dbContext.SaveChanges();

            return Ok("User details updated successfully");

        }


        

        [HttpPost("AddToCart")]
        public IActionResult AddToCart( int productId, int quantity)
        {
            
            var product = _dbContext.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                return NotFound("Product not found");
            }

        
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Unauthorized("User not logged in");
            }

       
            var existingOrder = _dbContext.Orders.FirstOrDefault(o => o.UserID == userId && !o.OrderStatus);
            if (existingOrder == null)
            {
                
                existingOrder = new Order
                {
                    UserID = userId.Value,
                    OrderStatus = false, 
                    TotalAmount = 0 
                };

                _dbContext.Orders.Add(existingOrder);
                _dbContext.SaveChanges();
            }

           
            existingOrder.TotalAmount += product.Price * quantity;

            
            _dbContext.Orders.Update(existingOrder);
            _dbContext.SaveChanges();

            
            var cartItem = new CartItems
            {
                UserId = userId.Value,
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Qty = quantity,
                Price = product.Price
            };

            var cart = _httpContextAccessor.HttpContext.Session.GetCart();
            cart.MyCartItems.Add(cartItem);
            _httpContextAccessor.HttpContext.Session.SetObject("Cart", cart);

            return Ok("Product added to cart and order created/updated");
        }

        [HttpPost("ConfirmPayment")]
        public IActionResult ConfirmPayment()
        {
           
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Unauthorized("User not logged in");
            }

           
            var existingOrder = _dbContext.Orders.FirstOrDefault(o => o.UserID == userId && !o.OrderStatus);
            if (existingOrder == null)
            {
                return BadRequest("No pending order found to confirm payment");
            }

           
            var cart = _httpContextAccessor.HttpContext.Session.GetCart();

           
            if (cart.MyCartItems.Count == 0)
            {
                return BadRequest("Cart is empty");
            }

          
            decimal discountPercentage = _b.CalculateDiscount(userId.Value);

            
            List<string> billLines = new List<string>();
            decimal totalPrice = 0;

           
            foreach (var cartItem in cart.MyCartItems)
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.ProductID == cartItem.ProductID);
                if (product == null)
                {
                    return NotFound($"Product with ID {cartItem.ProductID} not found");
                }

                if (product.StockQuantity < cartItem.Qty)
                {
                    return BadRequest($"Insufficient stock for {product.ProductName}. Available: {product.StockQuantity}, Requested: {cartItem.Qty}");
                }

               
                product.StockQuantity -= cartItem.Qty;
                _dbContext.Products.Update(product);

                var orderDetail = new OrderDetail
                {
                    OrderID = existingOrder.OrderID,
                    ProductID = cartItem.ProductID,
                    UserID = userId.Value,
                    Quantity = cartItem.Qty,
                    UnitPrice = cartItem.Price
                };

                _dbContext.OrderDetails.Add(orderDetail);

                
                decimal itemTotal = cartItem.Qty * cartItem.Price;
                totalPrice += itemTotal;

               
                billLines.Add($"Product: {product.ProductName}, Quantity: {cartItem.Qty}, Unit Price: {cartItem.Price:C}, Total: {itemTotal:C}");
            }

           
            decimal discountedPrice = totalPrice * (1 - discountPercentage);
            existingOrder.TotalAmount = discountedPrice;

            
            _dbContext.SaveChanges();

            
            cart.MyCartItems.Clear();
            _httpContextAccessor.HttpContext.Session.SetObject("Cart", cart);

            
            existingOrder.OrderStatus = true;
            _dbContext.Orders.Update(existingOrder);
            _dbContext.SaveChanges();

            
            billLines.Add($"\nTotal Price (before discount): {totalPrice:C}");
            billLines.Add($"Discount Applied: {discountPercentage:P0}");
            billLines.Add($"Total Price (after discount): {discountedPrice:C}");
            billLines.Add("\nThank you for shopping with us! Enjoy your discount on future orders if eligible.");

            
            string billFileName = $"Bill_Order_{existingOrder.OrderID}.txt";
            string billContent = string.Join(Environment.NewLine, billLines);
            byte[] billBytes = System.Text.Encoding.UTF8.GetBytes(billContent);

            
            return File(billBytes, "text/plain", billFileName);
        }
        [HttpGet("TopTrendingItems")]
        public IActionResult TopTrendingItems()
        {
           
            var topTrendingItems = _dbContext.OrderDetails
                .GroupBy(od => od.ProductID)
                .Select(g => new
                {
                    ProductID = g.Key,
                    PurchaseCount = g.Count()
                })
                .OrderByDescending(x => x.PurchaseCount)
                .Take(5)  
                .ToList();

            if (!topTrendingItems.Any())
            {
                return NotFound("No trending items found.");
            }

          
            var topProducts = topTrendingItems
                .Select(item => new
                {
                    Product = _dbContext.Products.FirstOrDefault(p => p.ProductID == item.ProductID),
                    item.PurchaseCount
                })
                .Where(x => x.Product != null)  
                .Select(x => new
                {
                    x.Product.ProductName,
                    x.PurchaseCount
                })
                .ToList();

            return Ok(topProducts);
        }



        [HttpGet("OrderDetails")]
        public IActionResult GetOrderDetails()
        {
           
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Unauthorized("User not logged in");
            }

           
            var orders = _dbContext.Orders
                .Where(o => o.UserID == userId && o.OrderStatus)
                .Select(o => new
                {
                    o.OrderID,
                    o.TotalAmount,
                    OrderDetails = _dbContext.OrderDetails.Where(od => od.OrderID == o.OrderID).ToList()
                })
                .ToList();

            return Ok(orders);
        }







        [HttpDelete("remove/{productId}")]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = _httpContextAccessor.HttpContext.Session.GetCart();
            var cartItem = cart.MyCartItems.FirstOrDefault(c => c.ProductID == productId);

            if (cartItem == null)
            {
                return NotFound("Product not found in cart");
            }

            cart.MyCartItems.Remove(cartItem);
            _httpContextAccessor.HttpContext.Session.SetObject("Cart", cart);

            return Ok("Product removed from cart");
        }
        //[HttpPost("ConfirmOrder")]
        //public IActionResult ConfirmOrder()
        //{
            
        //    // Get the user ID from the session
        //    int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
        //    if (userId == null)
        //    {
        //        return Unauthorized("User not logged in");
        //    }
        //    var cart = _httpContextAccessor.HttpContext.Session.GetCart();
          
        //    var newOrder = new Order
        //    {
        //        UserID = userId.Value,
        //      //  OrderDate = DateTime.Now
        //    };
        //    _dbContext.Orders.Add(newOrder);
        //    _dbContext.SaveChanges();
          
        //    foreach (var cartItem in cart.MyCartItems)
        //    {
        //        var product = _dbContext.Products.FirstOrDefault(p => p.ProductID == cartItem.ProductID);
        //        if (product == null)
        //        {
        //            return NotFound($"Product with ID {cartItem.ProductID} not found");
        //        }
        //        if (product.StockQuantity < cartItem.Qty)
        //        {
        //            return BadRequest($"Insufficient stock for {product.ProductName}. Available: {product.StockQuantity}, Requested: {cartItem.Qty}");
        //        }
                
        //        product.StockQuantity -= cartItem.Qty;
        //        //---------------------//
        //        var orderDetail = new OrderDetail();
        //        // orderDetail = new OrderDetail
        //        //{
        //        orderDetail.OrderID = newOrder.OrderID;
        //        orderDetail.ProductID = cartItem.ProductID;
        //        orderDetail.UserID = userId.Value;
        //        orderDetail.Quantity = cartItem.Qty;
        //        orderDetail.UnitPrice = cartItem.Price;
        //        // };
        //        _dbContext.OrderDetails.Add(orderDetail);
        //        _dbContext.SaveChanges();
        //    }

            



        //    cart.MyCartItems.Clear();
        //    _httpContextAccessor.HttpContext.Session.SetObject("Cart", cart);
        //    return Ok("Order confirmed successfully");
        //}

    }
}
