using ShopEaseApp.Models;
using static ShopEaseApp.Models.ShoppingDataContext;
using static ShopEaseApp.Models.ShoppingModel;
using Microsoft.EntityFrameworkCore;
namespace ShopEaseApp.Areas.Buyers.Models
{
    public interface Buyer
    {
        public ShoppingModel.Product search(string name);

        

    }

    public class Buyer1 : Buyer
    {
        ShoppingDataContext _dbContext;
        public Buyer1(ShoppingDataContext context)
        {
            _dbContext = context;
        }

       

        public ShoppingModel.Product search(string name)
        {
            ShoppingModel.Product pro = new ShoppingModel.Product();
            //  Product p= _dbContext.Products.Where(p => p.ProductName == name).Single();


            List<ShoppingModel.Product> plist = _dbContext.Products.ToList();
            foreach (var item in plist)
            {
                if (item.ProductName == name)
                {
                    pro = item;
                }

            }
            return pro;

        }


    }
}
