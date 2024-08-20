using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ShopEaseApp.Controllers
{
    [Authorize(Roles = "Seller")]
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Seller endpoint" });
        }
    }
}
