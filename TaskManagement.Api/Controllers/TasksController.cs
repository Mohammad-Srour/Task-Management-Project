using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Api.Dtos;
using TaskManagement.Api.Models.Entities;
using TaskManagement.Api.Models.Enums;
using TaskManagement.Api.Repositories;

namespace TaskManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskRepository taskRepository, ILogger<TasksController> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        private Guid GetUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                throw new UnauthorizedAccessException("User not authorized");
            return Guid.Parse(userIdStr);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks(
            [FromQuery]TaskManagement.Api.Models.Enums.TaskStatus? status,
            [FromQuery] TaskPriority? priority,
            [FromQuery] DateTime? dueDate)
        {
            try
            {
                var userId = GetUserId();
                var tasks = await _taskRepository.GetTasksByUserAsync(userId);

                if (status.HasValue)
                    tasks = tasks.Where(t => t.Status == status.Value);
                if (priority.HasValue)
                    tasks = tasks.Where(t => t.Priority == priority.Value);
                if (dueDate.HasValue)
                    tasks = tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == dueDate.Value.Date);

                _logger.LogInformation("User {UserId} retrieved {Count} tasks", userId, tasks.Count());

                return Ok(tasks);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid data");

            try
            {
                var userId = GetUserId();

                var task = new TaskItem
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Status = dto.Status,
                    Priority = dto.Priority,
                    DueDate = dto.DueDate,
                    UserId = userId
                };

                var createdTask = await _taskRepository.AddTaskAsync(task);

                _logger.LogInformation("User {UserId} created task {TaskId}", userId, createdTask.Id);
                return Ok(createdTask);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            try
            {
                var userId = GetUserId();
                var task = await _taskRepository.GetTaskByIdAsync(id, userId);

                if (task == null)
                    return NotFound("Task not found");

                return Ok(task);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskCreateDto dto)
        {
            try
            {
                var userId = GetUserId();

                var task = await _taskRepository.GetTaskByIdAsync(id, userId);
                if (task == null)
                    return NotFound("Task not found");

                task.Title = dto.Title;
                task.Description = dto.Description;
                task.Status = dto.Status;
                task.Priority = dto.Priority;
                task.DueDate = dto.DueDate;
                task.UpdatedAt = DateTime.UtcNow;

                var updated = await _taskRepository.UpdateTaskAsync(task);

                return Ok(updated);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> MarkComplete(Guid id)
        {
            try
            {
                var userId = GetUserId();

                var task = await _taskRepository.GetTaskByIdAsync(id, userId);
                if (task == null)
                    return NotFound("Task not found");

                task.IsCompleted = true;
                task.Status = TaskManagement.Api.Models.Enums.TaskStatus.Done;
                task.UpdatedAt = DateTime.UtcNow;

                var updated = await _taskRepository.UpdateTaskAsync(task);
                return Ok(updated);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPatch("{id}/incomplete")]
        public async Task<IActionResult> MarkIncomplete(Guid id)
        {
            try
            {
                var userId = GetUserId();

                var task = await _taskRepository.GetTaskByIdAsync(id, userId);
                if (task == null)
                    return NotFound("Task not found");

                task.IsCompleted = false;
                task.Status = TaskManagement.Api.Models.Enums.TaskStatus.New;
                task.UpdatedAt = DateTime.UtcNow;

                var updated = await _taskRepository.UpdateTaskAsync(task);
                return Ok(updated);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            try
            {
                var userId = GetUserId();

                var deleted = await _taskRepository.DeleteTaskAsync(id, userId);
                if (!deleted)
                    return NotFound("Task not found");

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
