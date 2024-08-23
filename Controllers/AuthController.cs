using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopEaseApp.Models;
using ShopEaseApp.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace ShopEaseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       // private readonly IAuthService _authService;
        private readonly IUserRepository _userrepo;
        IHttpContextAccessor _httpcontext;
        public AuthController(IUserRepository userrepo,IHttpContextAccessor httpContext)
        {
            //_authService = authService;
            _userrepo = userrepo;
            _httpcontext = httpContext; 
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            bool isValiduser = _userrepo.LoginAsync(model.Username, model.Password);
            if (isValiduser)
            {
                

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                    audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                    claims: new List<Claim>(new Claim[]
                    {
                        new Claim(ClaimTypes.Name,model.Username),
                        new Claim(ClaimTypes.Role,model.Role)
                    }),
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                ///create a cookie to store the token
                CookieOptions options = new CookieOptions
                {
                    Domain = "localhost", // Set the domain for the cookie
                    Expires = DateTime.Now.AddMinutes(6), // Set expiration date to 7 days from now
                    Path = "/", // Cookie is available within the entire application
                    Secure = true, // Ensure the cookie is only sent over HTTPS
                    HttpOnly = true, // Prevent client-side scripts from accessing the cookie
                  //  MaxAge = TimeSpan.FromMinutes(6), // Another way to set the expiration time
                    IsEssential = true // Indicates the cookie is essential for the application to function
                };
                //Response.Cookies.Append("UserId", "1234567", options);
               int userid= _userrepo.getUserIdbyName(model.Username);
                        
                Response.Cookies.Append(model.Username, userid.ToString());
                Response.Cookies.Append("JWTToken", tokenString, options);

                _httpcontext.HttpContext.Session.SetInt32("UserID", userid);
              //  HttpContext.Session.SetInt32("UserID", userid);

                  return Ok(new JWTTokenResponse { Token = tokenString });
                

            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {


var result= await _userrepo.RegisterUserAsync(model);
            if (result)
                return Ok(new { Message = "User registered successfully" });
            return BadRequest(new { Message = "User registration failed" });
        }
    }
}
