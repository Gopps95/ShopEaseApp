using Microsoft.AspNetCore.Identity;

namespace ShopEaseApp.Models
{
   
        public class ApplicationUser : IdentityUser
        {
            public string FullName { get; set; }
        }
    
}
