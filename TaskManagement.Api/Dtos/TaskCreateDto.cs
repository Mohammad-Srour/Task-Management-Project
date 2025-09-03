using System.ComponentModel.DataAnnotations;
using TaskManagement.Api.Models.Enums;

namespace TaskManagement.Api.Dtos
{
    public class TaskCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description can be up to 500 characters")]
        public string? Description { get; set; }

        [Required]
        public TaskManagement.Api.Models.Enums.TaskStatus Status { get; set; } = TaskManagement.Api.Models.Enums.TaskStatus.New;

        [Required]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
    }
}
