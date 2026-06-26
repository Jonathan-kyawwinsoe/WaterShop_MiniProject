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
    [Tags("Customer")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtProvider _jwtProvider;

        public CustomerController(AppDbContext context, PasswordHasher passwordHasher, JwtProvider jwtProvider)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }
        [HttpGet("Customer")]
        [EndpointSummary("Get all active customer")]
        [EndpointDescription("Customer all data")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<GetCustomerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<GetCustomerResponse>>> GetAllCustomer(
            [FromServices] AppDbContext db)
        {
            var customer = await db.Customers
                .Where(c => !c.IsDeleted)
                .Select(c => new GetCustomerResponse(
                    c.Id,
                    c.CustomerName,
                    c.PhoneNumber,
                    c.Address,
                    c.EmptyBottleOwed))
                .ToListAsync();
            return Ok(customer);
        }
        [HttpGet("{id:int}")]
        [EndpointSummary("Get active customer by Id")]
        [EndpointDescription("Get single customer by Id")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetCustomerResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetCustomerResponse>> GetCustomerById(
            int id,
            [FromServices] AppDbContext db
            )
        {
            var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (customer == null)
            {
                return Problem(
                    title: "Not Found",
                    detail: "Customer is not found",
                    statusCode: StatusCodes.Status400BadRequest
                    );
            }
            return Ok(new GetCustomerResponse(
                customer.Id,
                customer.CustomerName,
                customer.PhoneNumber,
                customer.Address,
                customer.EmptyBottleOwed
            ));
        }
        [HttpPost]
        [EndpointSummary("Create Customer")]
        [EndpointDescription("create new Customer")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreateCustomerResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CreateCustomerResponse>> CreateCustomer(
            [FromBody] CreateCustomerRequest request,
            [FromServices] AppDbContext db
            )
        {
            var customer = new Entity.Customer
            {
                CustomerName = request.CustomerName.Trim(),
                PhoneNumber = request.Phone.Trim(),
                Address = request.Address.Trim(),
                EmptyBottleOwed = 0
            };
            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerById),
                new { id = customer.Id },
                new CreateCustomerResponse(
                    customer.Id,
                    customer.CustomerName,
                    customer.PhoneNumber,
                    customer.Address
                ));

        }
        [HttpPut("{id:int}")]
        [EndpointSummary("Update and execting Customer")]
        [EndpointDescription("Update Customer By Id")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UpdateCustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UpdateCustomerResponse>> UpdateCustomer(
            int id,
            [FromBody] UpdateCustomerRequest request,
            [FromServices] AppDbContext db
            )
        {
            var customer = await db.Customers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if(customer is null)
            {
                return Problem(
                    title: "Not Found",
                    detail: "Customer not found",
                    statusCode: StatusCodes.Status404NotFound
                    );
            }
            customer.CustomerName = request.CustomerName.Trim();
            customer.PhoneNumber = request.phoneNumber.Trim();
            customer.Address = request.Address.Trim();

            await db.SaveChangesAsync();

            return Ok(new UpdateCustomerResponse(
                customer.Id,
                customer.CustomerName,
                customer.PhoneNumber,
                customer.Address
            ));
        }
        [HttpDelete("{id:int}")]
        [EndpointSummary("Delete Customer")]
        [EndpointDescription("Soft delete Customer")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DeleteCustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<DeleteCustomerResponse>> DeleteCustomer(
            int id,
            [FromServices] AppDbContext db 
            )
        {
            var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if(customer is null)
            {
                return Problem(
                    title:"Not Found",
                    detail:"Customer not found",
                    statusCode:StatusCodes.Status404NotFound
                );
            }
            customer.IsDeleted = true;
            await db.SaveChangesAsync();

            return Ok(new DeleteCustomerResponse(
                Message: "Customer Delete Successfully"
                ));
        }
    }
    
}
