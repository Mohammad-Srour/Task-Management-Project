using TaskManagement.Api.Models.Enums;

namespace TaskManagement.Api.Dtos
{
    public class TaskCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public TaskManagement.Api.Models.Enums.TaskStatus Status { get; set; }
            = TaskManagement.Api.Models.Enums.TaskStatus.New; public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime? DueDate { get; set; }

    }
}
