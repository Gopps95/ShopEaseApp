using Microsoft.EntityFrameworkCore;

using ShopEaseApp.Models;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

namespace ShopEaseApp.Repositories

{

   

    public interface IUserRepository

    {

        Task<bool> RegisterUserAsync(User user); 

        bool LoginAsync(string username, string password); 

        int GetUserIdByName(string username); 
    }

    

    public class SellerRepository : IUserRepository

    {

        private readonly ShoppingDataContext.ShoppingModelDB _context;

        public SellerRepository(ShoppingDataContext.ShoppingModelDB context)

        {

            _context = context;

        }

        

        public async Task<bool> RegisterUserAsync(User user)

        {

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _context.User.AddAsync(user);

            return await _context.SaveChangesAsync() > 0;

        }

        

        public bool LoginAsync(string username, string password)

        {

            User user = _context.User.FirstOrDefault(x => x.UserName == username);

            return user != null && BCrypt.Net.BCrypt.Verify(password, user.Password);

        }

       

        public int GetUserIdByName(string username)

        {

            User user = _context.User.SingleOrDefault(u => u.UserName == username);

            return user?.UserID ?? -1; 

        }

    }

  

    public class BuyerRepository : IUserRepository

    {

        private readonly ShoppingDataContext.ShoppingModelDB _context;

        public BuyerRepository(ShoppingDataContext.ShoppingModelDB context)

        {

            _context = context;

        }

        

        public async Task<bool> RegisterUserAsync(User user)

        {

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _context.User.AddAsync(user);

            return await _context.SaveChangesAsync() > 0;

        }

      

        public bool LoginAsync(string username, string password)

        {

            User user = _context.User.FirstOrDefault(x => x.UserName == username);

            return user != null && BCrypt.Net.BCrypt.Verify(password, user.Password);

        }

      

        public int GetUserIdByName(string username)

        {

            User user = _context.User.SingleOrDefault(u => u.UserName == username);

            return user?.UserID ?? -1; 

        }

    }

}

