using System.ComponentModel.DataAnnotations;

namespace ShopEaseApp.Models
{
    public class UserModel
    {
        //public const string Buyer = "Buyer";


        //public const string Seller = "Seller";


        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }



        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }
}
