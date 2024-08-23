using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShopEaseApp.Areas.Buyers.Models;
using ShopEaseApp.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopEaseApp.Areas.Buyers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {

        const string SessionUserId = "_UserId";
        const string SessionUserName = "_UserName";
        private readonly Buyer _b;

        public BuyerController (Buyer b)
        {
            _b = b;
        }
        // GET: api/<BuyerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BuyerController>/5
        [HttpGet("{name}")]
        public ShoppingModel.Product find(string name )
        {
            ShoppingModel.Product h =   _b.search(name);
            return h;
        }

        // POST api/<BuyerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPost("AddCookies")]
        public void AddtoCookie([FromBody] ShoppingModel.Product h )
        {
            var options = new CookieOptions
            {
                Domain = "example.com", // Adjust as needed
                Expires = DateTime.Now.AddDays(7), // Set expiration date
                Path = "/", // Cookie is available across the entire site
                Secure = true, // Only sent over HTTPS
                HttpOnly = true, // Prevents access via JavaScript
                MaxAge = TimeSpan.FromDays(7), // Sets the maximum age of the cookie
                IsEssential = true // Indicates the cookie is essential
            };
            Response.Cookies.Append(h.ProductID.ToString(), h.ProductName, options); ;

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
