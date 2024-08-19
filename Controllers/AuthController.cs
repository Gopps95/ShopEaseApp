using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopEaseApp.Models;



namespace ShopEaseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _authService.LoginAsync(model);
            if (token == null)
                return Unauthorized();
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            var result = await _authService.RegisterAsync(model);
            if (result)
                return Ok(new { Message = "User registered successfully" });
            return BadRequest(new { Message = "User registration failed" });
        }
    }
}
