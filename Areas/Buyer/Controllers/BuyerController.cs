using Microsoft.AspNetCore.Mvc;
using ShopEaseApp.Areas.Buyer.Models;
using ShopEaseApp.Areas.Seller.Models;
using ShopEaseApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopEaseApp.Areas.Buyer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        BuyerModel _b;
        public BuyerController(BuyerModel b)
        {
            _b=b;
        }
        // GET: api/<BuyerController>
        //[HttpGet]
        //public List<CartItems> Get()
        //{
        //    List<CartItems> myPurchase = HttpContext.Session.GetCart().MyCartItems;
        //    return myPurchase;
        //}
        [HttpGet("{id}")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/<BuyerController>/5
        [HttpGet("name")]
        public Product Get(string name)
        {
            
            return _b.Search(name);
        }

        // POST api/<BuyerController>
        //[HttpPost]
        //public IActionResult Post([FromBody] CartItems item)
        //{
        //    var cart = HttpContext.Session.GetCart();
        //    cart.MyCartItems.Add(item);
        //    HttpContext.Session.SetObject("Cart", cart);
        //    return Ok("Product Added to Cart");
        //}

        [HttpPost("{id}")]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BuyerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BuyerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
