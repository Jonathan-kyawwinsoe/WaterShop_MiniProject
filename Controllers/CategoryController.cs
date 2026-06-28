using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using water_shop.Data;
using water_shop.DTO;
using water_shop.Services;

namespace water_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Category")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("Create Category")]
        [EndpointSummary("Create Category")]
        [EndpointDescription("Create new category")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreateCategoryResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateCategoryResponse>> CreateCategory(
            [FromBody] CreateCategoryRequest request,
            [FromServices] AppDbContext db
            )
        {
            var category = new Entity.Category { CategoryName = request.CategoryName.Trim() };
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return Ok(new CreateCategoryResponse(
                category.Id,
                category.CategoryName,
                category.CreatedAt
                ));
        }
        [HttpPut("{id:int}")]
        [EndpointSummary("Update and execting Category")]
        [EndpointDescription("Update category by Id")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UpdateCategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UpdateCategoryResponse>> UpdateCategory(
            int id,
            [FromBody] UpdateCategoryRequest request, 
            [FromServices] AppDbContext db
            )
        {
            var category = await db.Categories.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
            if(category is null)
            {
                return Problem(
                    title:"Not Found",
                    detail:"Category not found by ID",
                    statusCode:StatusCodes.Status404NotFound
                );
            }
            category.CategoryName = request.CategoryName.Trim();

            await db.SaveChangesAsync();

            return Ok(new UpdateCategoryResponse(
                category.Id,
                category.CategoryName,
                category.UpdatedAt
            ));
        }
        [HttpGet("Get Category")]
        [EndpointSummary("Get all category")]
        [EndpointDescription("Category all data")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<GetCategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetCategoryResponse>> GetAllData(
            [FromServices] AppDbContext db
            )
        {
            var category = await db.Categories
                .Where(c => !c.IsDeleted)
                .Select(c => new GetCategoryResponse(
                    c.Id,
                    c.CategoryName,
                    c.CreatedAt,
                    c.UpdatedAt
                ))
                .ToListAsync();
            return Ok(category);
        }
        [HttpGet("{id:int}")]
        [EndpointSummary("Get category by Id")]
        [EndpointDescription("Get single category by id")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<GetCategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetCategoryResponse>> GetCategoryById(
            int id,
            [FromServices] AppDbContext db
            )
        {
            var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if(category is null)
            {
                return Problem(
                    title: "Not Found",
                    detail: "Category not found by ID",
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            return Ok(new GetCategoryResponse(
                category.Id,
                category.CategoryName,
                category.CreatedAt,
                category.UpdatedAt
                ));
        }
        [HttpDelete("{id:int}")]
        [EndpointSummary("Delete Category")]
        [EndpointDescription("Soft delete Category")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DeleteCategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<DeleteCategoryResponse>> DeleteCategoryByid(
            int id,
            [FromServices] AppDbContext db
            )
        {
            var Category = await db.Categories.FirstOrDefaultAsync(c => c.Id==id && !c.IsDeleted);
            if(Category is null)
            {
                return Problem(
                    title: "Not Found",
                    detail: "Customer not found",
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            Category.IsDeleted = true;
            await db.SaveChangesAsync();

            return Ok(new DeleteCategoryResponse(
                Message: "Category delete successfully"
                ));
        }

    }
}
