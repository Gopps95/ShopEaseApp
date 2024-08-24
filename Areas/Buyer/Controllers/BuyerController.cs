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

            // Create a new CartItem
            var cartItem = new CartItems
            {
                UserId = userId.Value,
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Qty = quantity,
                Price = product.Price
            };

            // Add the item to the cart
            var cart = _httpContextAccessor.HttpContext.Session.GetCart();
            cart.MyCartItems.Add(cartItem);
            _httpContextAccessor.HttpContext.Session.SetObject("Cart", cart);

            return Ok("Product added to cart");
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
        [HttpPost("ConfirmOrder")]
        public IActionResult ConfirmOrder()
        {
            
            // Get the user ID from the session
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return Unauthorized("User not logged in");
            }
            var cart = _httpContextAccessor.HttpContext.Session.GetCart();
          
            var newOrder = new Order
            {
                UserID = userId.Value,
              //  OrderDate = DateTime.Now
            };
            _dbContext.Orders.Add(newOrder);
            _dbContext.SaveChanges();
          
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
                //---------------------//
                var orderDetail = new OrderDetail();
                // orderDetail = new OrderDetail
                //{
                orderDetail.OrderID = newOrder.OrderID;
                orderDetail.ProductID = cartItem.ProductID;
                orderDetail.UserID = userId.Value;
                orderDetail.Quantity = cartItem.Qty;
                orderDetail.UnitPrice = cartItem.Price;
                // };
                _dbContext.OrderDetails.Add(orderDetail);
                _dbContext.SaveChanges();
            }

            



            cart.MyCartItems.Clear();
            _httpContextAccessor.HttpContext.Session.SetObject("Cart", cart);
            return Ok("Order confirmed successfully");
        }

    }
}
