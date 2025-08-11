using BudgetTracker.Application.Interfaces.Category;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Category.Requests;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private Guid? UserId => User.GetUserId();
    private readonly ICategoryService _categoryService;
    
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpPost]
    [ProducesResponseType<CategoryDto>(200)]
    public async Task<IActionResult> CreateAsync([FromBody]CreateCategory request, CancellationToken cancellation)
    {
        return Ok(await _categoryService.CreateAsync(request, cancellation));
    }
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<CategoryDto>>(200)]
    public async Task<IActionResult> GetUserCategoriesAsync(CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _categoryService.GetUserCategoriesAsync(UserId.Value, cancellation));
    }
    
    [HttpPatch]
    [ProducesResponseType<CategoryDto>(200)]
    public async Task<IActionResult> UpdateCategoryAsync(string name, Guid categoryId, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _categoryService.UpdateCategoryAsync(name, categoryId, UserId.Value, cancellation));
    }
    
    [HttpGet("{categoryId:guid}")]
    [ProducesResponseType<CategoryDto>(200)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid categoryId, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _categoryService.GetByIdAsync(categoryId, UserId.Value, cancellation));
    }
}