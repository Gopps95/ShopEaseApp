using Newtonsoft.Json;

namespace ShopEaseApp.Areas.Seller.Models
{
    public static class SessionExtensions
    {
        public static Cart GetCart(this ISession session)
        {
            var cart = session.GetObject<Cart>("Cart");
            if (cart == null)
            {
                cart = new Cart();
                session.SetObject("Cart", cart);

            }
            return cart;
        }
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));

        }
        public static T GetObject<T>(this ISession session, string key)
        {
            var obj = session.GetString(key);
            //      var value =JsonConvert.DeserializeObject<T>(obj);
            return obj == null ? default(T) : JsonConvert.DeserializeObject<T>(obj);
        }
    }
    public class Cart
    {
        public List<CartItems> MyCartItems { get; set; } = new List<CartItems>();
    }
    public class CartItems
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
}