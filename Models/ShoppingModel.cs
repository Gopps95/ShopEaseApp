using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShopEaseApp.Models
{
    
        [Table("User")]
        public class User
        {
            [Key]
            public int UserID { get; set; }

            [Required]
            [StringLength(50)]
            [MinLength(10)]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.EmailAddress)]
            public string EmailID { get; set; }

            [Required]
            [StringLength(50)]
            [MinLength(10)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            public string Role { get; set; }

            // Navigation property for orders
        }


        [Table("Products")]
        public class Product
        {
            [Key]
            public int ProductID { get; set; }

            [Required]
            [StringLength(50)]
            [MinLength(10)]
            public string ProductName { get; set; }

            [Required]
            [StringLength(50)]
            [MinLength(10)]
            public string ProductDescription { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            public decimal Price { get; set; }

            [Required]
            public int StockQuantity { get; set; }


            //[ForeignKey("User")]
            public int? UserID { get; set; }

            // Navigation property
         //   public User User { get; set; }
            // public ICollection<OrderDetail> OrderDetails { get; set; }
        }

        [Table("Orders")]
        public class Order
        {
            [Key]
            public int OrderID { get; set; }

            //[Required]
            //[ForeignKey("User")]
            public int? UserID { get; set; }

            [DataType(DataType.Currency)]
            public decimal TotalAmount { get; private set; }
            public bool OrderStatus { get; set; }

            // Navigation properties
           public User User { get; set; }
        }


        [Table("OrderDetails")]
        [Keyless]
        public class OrderDetail
        {
            // [NotMapped()]
            // [Key()]
            //public int OrderDetailsID { get; set; }

            //[ForeignKey("Orders")]
            public int? OrderID { get; set; }

            //[ForeignKey("Products")]
            public int? ProductID { get; set; }

            [Required]
            public int Quantity { get; set; }

            public int UnitPrice { get; set; }

            // Navigation properties

            public Order Order { get; set; }
           public Product Product { get; set; }



        }

        [Table("Payments")]
        public class Payment
        {
            [Key]
            [Required]
            public int PaymentID { get; set; }

            //[ForeignKey("Orders")]
            public int? OrderID { get; set; }

            public int Amount { get; private set; }

            public bool PaymentStatus { get; set; }

            public string PaymentMethod { get; set; }

            public Order Order { get; set; }
        }

        [Table("Category")]
        public class Category
        {
            [Key]
            public int CategoryID { get; set; }

            [Required]
            public string CategoryName { get; set; }

            [Required]
            public string CategoryDescription { get; set; }
        }
    
   
}
