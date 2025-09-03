using TaskManagement.Api.Models.Entities;

namespace TaskManagement.Api.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesByUserAsync(Guid userId);
        Task<Category?> GetCategoryByIdAsync(Guid id, Guid userId);
        Task<Category> AddCategoryAsync(Category category);
        Task<Category?> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Guid id, Guid userId);
    }
}
