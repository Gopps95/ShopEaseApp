using ShopEaseApp.Models;

using Microsoft.EntityFrameworkCore;

using ShopEaseApp.Models;
namespace ShopEaseApp.Areas.Buyer.Models
{
    public interface IBuyer
    {

        public ShoppingModel.Product search(string name );

      

    }

    public class ProductRepository : IBuyer
    {

         private readonly ShoppingModelDB _context;
        public ProductRepository(ShoppingDataContext context)
        {
            _context = context;

        }

        public ShoppingModel.Product search(string name )
        { 
           ShoppingModel.Product product = new ShoppingModel.Product();

            foreach(var i in _context.Products)
            {
               if (i.ProductName == name )
              {
                    product = i;
                }
           }

          return product;

        }


        








    }
}