using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ShopEaseApp.Controllers
{
   
   
        [Authorize(Roles = "Buyer")]
        [Route("api/[controller]")]
        [ApiController]
        public class BuyerController : ControllerBase
        {
            [HttpGet]
            public IActionResult Get()
            {
                return Ok(new { Message = "Buyer endpoint" });
            }
        }

    
}
