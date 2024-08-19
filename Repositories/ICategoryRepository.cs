using Microsoft.EntityFrameworkCore;

namespace ShopEaseApp.Models
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<bool> AddCategoryAsync(Category category);

    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ShoppingDataContext.ShoppingModelDB _context;

        public CategoryRepository(ShoppingDataContext.ShoppingModelDB context)
        {
            _context = context;
        }
        public async Task<bool> AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            return await _context.SaveChangesAsync() > 0;
            // throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();

            //throw new NotImplementedException();
        }
    }
}
