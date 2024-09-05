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
            
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.EmailAddress)]
            public string EmailID { get; set; }

            [Required]
            
            [MinLength(10)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            public string Role { get; set; }

            
        }


        [Table("Products")]
        public class Product
        {
            [Key]
            public int ProductID { get; set; }

            [Required]
            [StringLength(50)]

            public string ProductName { get; set; }

            [Required]
            [StringLength(50)]
           
            public string ProductDescription { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            public decimal Price { get; set; }

            [Required]
            public int StockQuantity { get; set; }


        

        
        int? _userid;
            public int? UserID 
        {
             set
            {
                _userid = value;
            }
             get { return _userid; }
        }

        }

        [Table("Orders")]
        public class Order
        {
            [Key]
            public int OrderID { get; set; }

           public int? UserID { get; set; }

            [DataType(DataType.Currency)]
            public decimal TotalAmount { get;  set; }
            public bool OrderStatus { get; set; }
            public int Quantity { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        
        public User User { get; set; }
        }


        [Table("OrderDetails")]
        
        public class OrderDetail
        {
            
            public int? OrderID { get; set; }

            
            public int? ProductID { get; set; }

            [Required]
            public int Quantity { get; set; }

            public decimal UnitPrice { get; set; }
            public int? UserID { get; set; }
            

           public Product Product { get; set; }
        public Order Order { get; set; }


    }

       
    
   
}
