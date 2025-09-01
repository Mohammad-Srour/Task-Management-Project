using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagement.Api.Dtos;
using TaskManagement.Api.Models.Entities;
using TaskManagement.Api.Models.Enums;

namespace TaskManagement.Api.Controllers
{
    [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TasksController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks(
     [FromQuery] TaskManagement.Api.Models.Enums.TaskStatus? status,
     [FromQuery] TaskPriority? priority,
     [FromQuery] DateTime? dueDate)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            var userId = Guid.Parse(userIdStr);

            var query = _context.Tasks
                .Where(t => t.UserId == userId); 

            if (status.HasValue) query = query.Where(t => t.Status == status.Value);
            if (priority.HasValue) query = query.Where(t => t.Priority == priority.Value);
            if (dueDate.HasValue) query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == dueDate.Value.Date);

            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            var userId = Guid.Parse(userIdStr);

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                UserId = userId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }


        [HttpGet("{id}")]
        public IActionResult GetTaskById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId.ToString() == userId);
            if (task == null) return NotFound("Task not found");

            return Ok(task);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(Guid id, [FromBody] TaskCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId.ToString() == userId);
            if (task == null) return NotFound("Task not found");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate;
            task.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok(task);
        }

        [HttpPatch("{id}/complete")]
        public IActionResult MarkComplete(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId.ToString() == userId);
            if (task == null) return NotFound("Task not found");

            task.IsCompleted = true;
            task.Status = Models.Enums.TaskStatus.Done;
            task.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok(task);
        }

        [HttpPatch("{id}/incomplete")]
        public IActionResult MarkIncomplete(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId.ToString() == userId);
            if (task == null) return NotFound("Task not found");

            task.IsCompleted = false;
            task.Status = Models.Enums.TaskStatus.New;
            task.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId.ToString() == userId);
            if (task == null) return NotFound("Task not found");

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return NoContent(); 
        }




    }
}
