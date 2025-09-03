using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Models.Entities;

namespace TaskManagement.Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesByUserAsync(Guid userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id, Guid userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(Category category)
        {
            var existing = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == category.Id && c.UserId == category.UserId);

            if (existing == null) return null;

            existing.Name = category.Name;
            existing.Description = category.Description;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id, Guid userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
