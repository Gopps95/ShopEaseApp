using Microsoft.AspNetCore.Mvc;
using ShopEaseApp.Areas.Buyer.Models;
using ShopEaseApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopEaseApp.Areas.Buyer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {

        IBuyer _buyer;
        public BuyerController(IBuyer buyer)
        {
           _buyer =buyer;
        }
        // GET: api/<BuyerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BuyerController>/5
        [HttpGet("{name}")]
        public ShoppingModel.Product Findpro (string name  )
        {
              return _buyer.search(name );
        }

        // POST api/<BuyerController>
        [HttpPost]
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
