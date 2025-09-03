using TaskManagement.Api.Models.Entities;
using TaskManagement.Api.Models.Enums;

namespace TaskManagement.Api.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetTasksByUserAsync(Guid userId);
        Task<TaskItem?> GetTaskByIdAsync(Guid taskId, Guid userId);
        Task<TaskItem> AddTaskAsync(TaskItem task);
        Task<TaskItem?> UpdateTaskAsync(TaskItem task);
        Task<bool> DeleteTaskAsync(Guid taskId, Guid userId);
    }
}
