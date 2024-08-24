using Microsoft.AspNetCore.Mvc;
using ShopEaseApp.Areas.Buyer.Models;
//using ShopEaseApp.Areas.Seller.Models;
using ShopEaseApp.Models;
using static ShopEaseApp.Models.ShoppingDataContext;
//using ShopEaseApp.Areas.Buyer.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopEaseApp.Areas.Buyer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        // GET: api/<BuyerController>
        //[HttpGet]
        //public List<CartItems> Get()
        //{
        //    List<CartItems> myPurchase = HttpContext.Session.GetCart().MyCartItems;
        //    return myPurchase;
        //}


        [HttpGet("DisplayCart")]
        public List<CartItems> Get()
        {
            var cartItems = _httpContextAccessor.HttpContext.Session.GetCart().MyCartItems;
            return cartItems;
        }


        // GET api/<BuyerController>/5
        [HttpGet("name")]
        public Product Get(string name)
        {
            
            return _b.Search(name);
        }

        // POST api/<BuyerController>
        //[HttpPost("buyer/product/AddtoCart")]
        //public IActionResult Post([FromBody] Buyer.Models.CartItems item)
        //{
        //    var cart = HttpContext.Session.GetCart();
        //    cart.MyCartItems.Add(item);
        //    HttpContext.Session.SetObject("Cart", cart);
        //    return Ok("Product Added to Cart");
        //}

        [HttpPost("AddToCart")]
        public IActionResult AddToCart( int productId, int quantity)
        {
            // Fetch product from the database using productId
            var product = _dbContext.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            // Get the user ID from the session
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Unauthorized("User not logged in");
            }

            // Check if an order already exists for the user with OrderStatus false
            var existingOrder = _dbContext.Orders.FirstOrDefault(o => o.UserID == userId && !o.OrderStatus);
            if (existingOrder == null)
            {
                // Create a new order with status false
                existingOrder = new Order
                {
                    UserID = userId.Value,
                    OrderStatus = false, // Initially false, to be updated on payment confirmation
                    TotalAmount = 0 // Initial total amount is 0
                };

                _dbContext.Orders.Add(existingOrder);
                _dbContext.SaveChanges();
            }

            // Create a new CartItem and add it to the order (calculate total amount)
            existingOrder.TotalAmount += product.Price * quantity;

            // Update the order's total amount
            _dbContext.Orders.Update(existingOrder);
            _dbContext.SaveChanges();

            // Store the cart item in the session for temporary storage
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
            // Get the user ID from the session
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Unauthorized("User not logged in");
            }

            // Retrieve the existing order with status false
            var existingOrder = _dbContext.Orders.FirstOrDefault(o => o.UserID == userId && !o.OrderStatus);
            if (existingOrder == null)
            {
                return BadRequest("No pending order found to confirm payment");
            }

            // Get the cart from the session
            var cart = _httpContextAccessor.HttpContext.Session.GetCart();

            // Check if cart is empty
            if (cart.MyCartItems.Count == 0)
            {
                return BadRequest("Cart is empty");
            }

            // Add order details and update stock
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

                product.StockQuantity -= cartItem.Qty; // Update stock quantity

                var orderDetail = new OrderDetail
                {
                    OrderID = existingOrder.OrderID,
                    ProductID = cartItem.ProductID,
                    UserID = userId.Value,
                    Quantity = cartItem.Qty,
                    UnitPrice = cartItem.Price
                };

                _dbContext.OrderDetails.Add(orderDetail);
                _dbContext.SaveChanges();
            }

            // Clear the cart after payment confirmation
            cart.MyCartItems.Clear();
            _httpContextAccessor.HttpContext.Session.SetObject("Cart", cart);

            // Update the order status to true
            existingOrder.OrderStatus = true;
            _dbContext.Orders.Update(existingOrder);
            _dbContext.SaveChanges();

            return Ok("Payment confirmed and order placed successfully");
        }

        [HttpGet("OrderDetails")]
        public IActionResult GetOrderDetails()
        {
            // Get the user ID from the session
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Unauthorized("User not logged in");
            }

            // Fetch all confirmed orders for the user
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






        // PUT api/<BuyerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
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
