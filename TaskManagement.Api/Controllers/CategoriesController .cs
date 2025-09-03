using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Api.Dtos;
using TaskManagement.Api.Models.Entities;
using TaskManagement.Api.Repositories;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryRepository categoryRepository, ILogger<CategoriesController> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

        var userId = Guid.Parse(userIdStr);
        var categories = await _categoryRepository.GetCategoriesByUserAsync(userId);

        return Ok(categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

        var userId = Guid.Parse(userIdStr);
        var category = await _categoryRepository.GetCategoryByIdAsync(id, userId);

        if (category == null) return NotFound();

        return Ok(new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)
    {
        if (dto == null) return BadRequest("Invalid data");

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

        var userId = Guid.Parse(userIdStr);
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            UserId = userId
        };

        var created = await _categoryRepository.AddCategoryAsync(category);

        return Ok(new CategoryDto
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryDto dto)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

        var userId = Guid.Parse(userIdStr);

        var category = new Category
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            UserId = userId
        };

        var updated = await _categoryRepository.UpdateCategoryAsync(category);

        if (updated == null) return NotFound("Category not found");

        return Ok(new CategoryDto
        {
            Id = updated.Id,
            Name = updated.Name,
            Description = updated.Description
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

        var userId = Guid.Parse(userIdStr);
        var deleted = await _categoryRepository.DeleteCategoryAsync(id, userId);

        if (!deleted) return NotFound("Category not found");

        return NoContent();
    }
}
