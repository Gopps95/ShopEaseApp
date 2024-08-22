using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ShopEaseApp.Areas.Seller.Models;
using ShopEaseApp.Models;
using ShopEaseApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopEaseApp.Areas.Seller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize(Roles = "Seller")]
    public class SellerController : ControllerBase
    {
        ISellerModel _sel;
        public SellerController(ISellerModel sel)
        {
            _sel = sel;
        }

        // GET: api/<SellerController>
        [HttpGet("/Products")]
        public IEnumerable<Product> GetAllProducts()
        {
            return _sel.GetAllProducts();
        }


        [HttpGet("Orders")]
        public IEnumerable<Order> GetAllOrders()
        {
            return (IEnumerable<Order>)_sel.GetAllOrders();
        }
        // GET api/<SellerController>/5
        [HttpGet("{id}")]
        public bool Confirmorder(int OrderID)
        {
            return true;
        }

        [HttpGet("{id}/products")]
        public bool ConfirmPayment(int products)
        {
            return true;
        }
        
        // POST api/<SellerController>
       
        [HttpPost("/AddProducts")]
       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public void PostProduct([FromBody]Product product)
        {
          if( Request.Cookies.TryGetValue("UserId", out string userId))
           {
                int userid = Convert.ToInt32(userId);
                HttpContext.Session.SetInt32("userid", userid);
                
                _sel.Create(product);
            }
            
        }

        // PUT api/<SellerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SellerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
