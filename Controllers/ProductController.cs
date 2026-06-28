using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using water_shop.Data;
using water_shop.DTO;

namespace water_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Tags("Product")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        public ProductController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        [HttpPost("Create Product")]
        [EndpointSummary("Create product")]
        [EndpointDescription("Create new Product")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreateProductResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateProductResponse>> CreateProduct(
            [FromBody] CreateProductRequest request,
            [FromServices] AppDbContext db
            )
        {
            var product = new Entity.Products { 
                ProductName = request.ProductName.Trim(),
                CategoryId = request.CategoryId,
                UnitsPrice = request.UnitsPrice,
                StockQuantity = request.SockQuantity,
                IsReturnAble = request.IsReturnAble,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            db.Products.Add(product);
            await db.SaveChangesAsync();

            return Ok(new CreateProductResponse(
                product.Id,
                product.ProductName,
                product.CategoryId,
                product.UnitsPrice,
                product.StockQuantity,
                product.IsReturnAble,
                product.CreatedAt
                ));
        }
        [HttpPut("{id:int}")]
        [EndpointSummary("Update product")]
        [EndpointDescription("Update Product")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UpdateProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateProductResponse>> UpdateProduct(
            int id,
            [FromBody] UpdateProductRequest request,
            [FromServices] AppDbContext db
            )
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if ( product == null )
            {
                return Problem(
                    title: "Not Found",
                    detail: "Category not found by ID",
                    statusCode: StatusCodes.Status404NotFound
                    );
            }
            product.ProductName = request.ProductName;
            product.CategoryId = request.CategoryId;
            product.UnitsPrice = request.UnitsPrice;
            product.StockQuantity = request.SockQuantity;
            product.IsReturnAble = request.IsReturnAble;
            product.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return Ok(new UpdateProductResponse(
                product.Id,
                product.ProductName,
                product.CategoryId,
                product.UnitsPrice,
                product.StockQuantity,
                product.IsReturnAble,
                product.UpdatedAt
                ));
        }
        [HttpGet("Get all Product")]
        [EndpointSummary("Get all product")]
        [EndpointDescription("Get  all  Product")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetProductResponse>> GetALlProduct(
            [FromServices] AppDbContext db
            )
        {
            var product = await db.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Select(p => new GetProductResponse(
                    p.Id,
                    p.ProductName,
                    p.CategoryId,
                    p.Category != null ? p.Category.CategoryName : null,
                    p.UnitsPrice,
                    p.StockQuantity,
                    p.IsReturnAble,
                    p.CreatedAt,
                    p.UpdatedAt
                ))
                .ToListAsync();
            return Ok(product);
        }
        [HttpGet("{id:int}")]
        [EndpointSummary("Get data by id")]
        [EndpointDescription("Get  data by id ")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetProductResponse>> GetProductById(
            int id,
            [FromServices] AppDbContext db
            )
        {
            var  product = await db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (product is null)
            {
                return Problem(
                    title: "Not Found",
                    detail: "Category not found by ID",
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            return Ok(new GetProductResponse(
                product.Id,
                product.ProductName,
                product.CategoryId,
                product.Category != null ? product.Category.CategoryName : null,
                product.UnitsPrice,
                product.StockQuantity,
                product.IsReturnAble,
                product.CreatedAt,
                product.UpdatedAt
                ));
        }
        [HttpDelete("{id:int}")]
        [EndpointSummary("Delete Product")]
        [EndpointDescription("Soft delete product")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DeleteProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DeleteProductResponse>> DeleteById(
            int id,
            [FromServices] AppDbContext db
            )
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if(product is null)
            {
                return Problem(
                    title: "Not Found",
                    detail: "Category not found by ID",
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            product.IsDeleted = true;
            await db.SaveChangesAsync();

            return Ok(new DeleteProductResponse(
                Message:"Delete Successfully Producr"
                ));
        }
    }
}
