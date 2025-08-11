using BudgetTracker.Application.Interfaces.Category;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Category.Requests;
using BudgetTracker.Domain.Models.Category;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpPost]
    [ProducesResponseType<CategoryDto>(200)]
    public async Task<IActionResult> CreateAsync([FromBody]CreateCategory request, CancellationToken cancellation)
    {
        return Ok(await _categoryService.CreateAsync(request,cancellation));
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<CategoryDto>>(200)]
    public async Task<IActionResult> GetUserCategoriesAsync(CancellationToken cancellation)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim is null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);
        return Ok(await _categoryService.GetUserCategoriesAsync(userId, cancellation));
    }
    
    [HttpPatch]
    [ProducesResponseType<CategoryDto>(200)]
    public async Task<IActionResult> UpdateCategoryAsync(string name,Guid categoryId, CancellationToken cancellation)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim is null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);
        return Ok(await _categoryService.UpdateCategoryAsync(name,categoryId,userId,cancellation));
    }
    
    [HttpGet("{categoryId:guid}")]
    [ProducesResponseType<CategoryDto>(200)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid categoryId, CancellationToken cancellation)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim is null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);
        return Ok(await _categoryService.GetByIdAsync(categoryId,userId,cancellation));
    }
}