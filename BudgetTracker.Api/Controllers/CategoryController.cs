using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Category;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Category.Requests;
using BudgetTracker.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private Guid? UserId => User.GetUserId();
    private readonly ICategoryService _categoryService;
    private readonly ISummaryService _summaryService;
    private readonly IValidator<CreateCategory> _createCategoryValidator;
    private readonly IValidator<UpdateCategory> _updateCategoryValidator;
    public CategoryController(ICategoryService categoryService, ISummaryService summaryService, IValidator<CreateCategory> createCategoryValidator, IValidator<UpdateCategory> updateCategoryValidator)
    {
        _categoryService = categoryService;
        _summaryService = summaryService;
        _createCategoryValidator = createCategoryValidator;
        _updateCategoryValidator = updateCategoryValidator;
    }
    
    [HttpPost]
    [ProducesResponseType<CategoryDto>(200)]
    public async Task<IActionResult> CreateAsync([FromBody]CreateCategory request, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        var validate = await _createCategoryValidator.ValidateAsync(request, cancellation);
        if (!validate.IsValid)
        {
            return BadRequest(validate.ToDictionary());
        }
        return Ok(await _categoryService.CreateAsync(request,UserId.Value, cancellation));
    }
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<CategoryDto>>(200)]
    public async Task<IActionResult> GetUserCategoriesAsync(CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _categoryService.GetUserCategoriesAsync(UserId.Value, cancellation));
    }
    
    [HttpPatch("{categoryId:guid}")]
    [ProducesResponseType<CategoryDto>(200)]
    public async Task<IActionResult> UpdateCategoryAsync(UpdateCategory request,[FromRoute] Guid categoryId, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        var validate = await _updateCategoryValidator.ValidateAsync(request, cancellation);
        if (!validate.IsValid)
        {
            return BadRequest(validate.ToDictionary());
        }
        return Ok(await _categoryService.UpdateCategoryAsync(request, categoryId, UserId.Value, cancellation));
    }
    
    [HttpGet("{categoryId:guid}")]
    [ProducesResponseType<CategoryDto>(200)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid categoryId, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _categoryService.GetByIdAsync(categoryId, UserId.Value, cancellation));
    }
    
    
    [HttpGet("top-expenses")]
    public async Task<IActionResult> GetTopExpensesCategoriesInMonthAsync(CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _summaryService.GetTopExpensesCategoryInMonthAsync(UserId.Value, cancellation));
    }
}