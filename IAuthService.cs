using ShopEaseApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using  ShopEaseApp.Models;
using ShopEaseApp.Helpers;
using Microsoft.Extensions.Options;
using ShopEaseApp.Repositories;
using ShopEaseApp.Controllers;

namespace ShopEaseApp
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(User model);
        Task<string> LoginAsync(LoginModel model);
    }
   // public class AuthService : IAuthService
    //{
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly JwtSettings _jwtSettings;
      //  private readonly IUserRepository _userrepo;

        //public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings,IUserRepository userrepo)
        //{
        //    _userManager = userManager;
        //    _jwtSettings = jwtSettings.Value;
        //    _userrepo = userrepo;
        //}

        //public AuthService(IOptions<JwtSettings> jwtSettings, IUserRepository userrepo)
        //{
        //    _jwtSettings = jwtSettings.Value;
        //        _userrepo = userrepo;

        //}

        //public async Task<string> LoginAsync(LoginModel model)
        //{
        // //   var user = await _userManager.FindByNameAsync(model.Username);
        //   // if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //   bool isValiduser= _userrepo.LoginAsync(model.Username, model.Password);
        //    if (isValiduser)
        //     {
        //        //var userRoles = await _userManager.GetRolesAsync(user);
        //        //var authClaims = new List<Claim>
        //        //{

        //        //    new Claim(user.),
        //        //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        //};

        //        //foreach (var userRole in userRoles)
        //        //{
        //        //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //        //}

        //        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
        //        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        //        var tokeOptions = new JwtSecurityToken(
        //            issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
        //            audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
        //            claims: new List<Claim>(),
        //            expires: DateTime.Now.AddMinutes(6),
        //            signingCredentials: signinCredentials
        //        );

        //        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        //        return Ok(new JWTTokenResponse { Token = tokenString });

        //    }
        //    return Unauthorized();
        //}

//        public async Task<bool> RegisterAsync(User model)
//        {
//           //var userExists = await _userManager.FindByNameAsync(model.UserName);
//           // if (userExists != null)
//           //     return false;

//            //ApplicationUser user = new ApplicationUser()
//            //{
//            //    Email = model.EmailID,
//            //    SecurityStamp = Guid.NewGuid().ToString(),
//            //    UserName = model.UserName
//            //    //FullName = model.FullName
//            //};

//            var result=await _userrepo.RegisterUserAsync(model);
//            return result;
////            var result = await _userManager.CreateAsync(user, model.Password);
//            //if (!result.Succeeded)
//            //    return false;

//          //  await _userManager.AddToRoleAsync(user, model.Role);
//          //  return true;
//        }
//    }
}
