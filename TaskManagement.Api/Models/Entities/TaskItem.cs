using TaskManagement.Api.Models.Enums;

namespace TaskManagement.Api.Models.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public TaskManagement.Api.Models.Enums.TaskStatus Status { get; set; }
            = TaskManagement.Api.Models.Enums.TaskStatus.New; public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

       
    
    }

}
