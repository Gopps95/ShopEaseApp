using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;

using System.Net;

using Microsoft.EntityFrameworkCore;

using ShopEaseApp.Areas.Seller.Models;

using ShopEaseApp.Models;

using ShopEaseApp.Controllers;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopEaseApp.Areas.Seller.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    [Authorize(Roles = "Seller")]

    public class SellerController : ControllerBase

    {

        ISellerModel _sel;

        IHttpContextAccessor _httpcontext;

        public SellerController(ISellerModel sel, IHttpContextAccessor httpcontext)

        {

            _sel = sel;

            _httpcontext = httpcontext;

        }

        // GET: api/<SellerController>

        [HttpGet("/Products")]

        public IEnumerable<Product> GetAllProducts()

        {

            return _sel.GetAllProducts();

        }


        //[HttpGet("Orders")]

        //public IEnumerable<Order> GetAllOrders()

        //{

        //    return (IEnumerable<Order>)_sel.GetAllOrders();

        //}

        // GET api/<SellerController>/5

        //[HttpGet("{id}")]

        //public bool Confirmorder(int OrderID)

        //{

        //    return true;

        //}

        //[HttpGet("{id}/products")]

        //public bool ConfirmPayment(int products)

        //{

        //    return true;

        //}

        // POST api/<SellerController>

        [HttpPost("/AddProducts")]

        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public void PostProduct([FromBody] Product product)

        {

            // if( Request.Cookies.TryGetValue("UserId", out string userId))

            // {

            //int userid = Convert.ToInt32(userId);

            int? UserID = _httpcontext.HttpContext.Session.GetInt32("UserID");

            //Product p1 = new Product();

            //p1.ProductName = product.ProductName;

            //p1.ProductDescription = product.ProductDescription; 

            //p1.Price=product.Price;

            //p1.StockQuantity = product.StockQuantity;


            //  product.UserID=HttpContext.Session.GetInt32("UserID");

            _sel.Create(product, UserID);

            //}

        }

        // PUT api/<SellerController>/5

        [HttpPut("updateproduct/{id}")]

        public IActionResult UpdateProduct(int id, [FromBody] Product product)

        {

            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserID");

            if (userId == null)

            {

                return Unauthorized("User not authenticated.");

            }

            bool result = _sel.Update(id, product, userId);

            if (!result)

            {

                return NotFound("Product not found or you are not the owner.");

            }

            return Ok("Product updated successfully.");

        }

        // DELETE api/<SellerController>/5

        [HttpDelete("product/delete/{id}")]

        public IActionResult DeleteProduct(int id)

        {

            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserID");

            if (userId == null)

            {

                return Unauthorized("User not authenticated.");

            }

            bool result = _sel.Delete(id, userId);

            if (!result)

            {

                return NotFound("Product not found or you are not the owner.");

            }

            return Ok("Product deleted successfully.");

        }

       

        [HttpGet("DownloadProductReport")]

        public IActionResult DownloadProductReport()

        {

            
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserID");

            if (userId == null)

            {

                return Unauthorized("User not authenticated.");

            }

            

            var sellerInfo = _sel.GetSellerInfo(userId, _httpcontext);

            if (sellerInfo.SellerId == 0)

            {

                return NotFound("Seller information not found.");

            }

           

            var productsBought = _sel.GetProductsBoughtFromSeller(userId);

            if (productsBought == null || !productsBought.Any())

            {

                return NotFound("No products found for the seller.");

            }

            
            StringBuilder reportContent = new StringBuilder();

            reportContent.AppendLine($"Seller ID: {sellerInfo.SellerId}");

            reportContent.AppendLine($"Seller Name: {sellerInfo.SellerName}");

            reportContent.AppendLine("Products Bought:");

            reportContent.AppendLine("ProductID\tProductName\tQuantity\tPrice\tTotal");

            foreach (var product in productsBought)

            {

                var total = product.StockQuantity * product.Price;

                reportContent.AppendLine($"{product.ProductID}\t{product.ProductName}\t{product.StockQuantity}\t{product.Price}\t{total}");

            }

            

            byte[] fileBytes = Encoding.UTF8.GetBytes(reportContent.ToString());

            var fileName = $"Seller_{sellerInfo.SellerName}_ProductReport.txt";

            

            return File(fileBytes, "text/plain", fileName);

        }

    }

}


