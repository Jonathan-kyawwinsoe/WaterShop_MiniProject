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
    [Tags("Admin Auth")]
    public sealed class AdminAuthController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtProvider _jwtProvider;

        public AdminAuthController(AppDbContext appDbContext, PasswordHasher passwordHasher, JwtProvider jwtProvider)
        {
            _appDbContext = appDbContext;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        [EndpointSummary("Admin Login")]
        [EndpointDescription("Admin login use user name and Password")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AdminLoginResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AdminLoginResponse>> AdminLogin(
            [FromBody] AdminLoginRequest request,
            [FromServices] AppDbContext db,
            [FromServices] PasswordHasher hasher,
            [FromServices] JwtProvider jwt)
        {
            if(string.IsNullOrWhiteSpace(request.UserName)|| string.IsNullOrWhiteSpace(request.Password))
            {
                return Problem(
                    title: "Invalid creadentials",
                    detail: "User Name and Password are require",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            var userName = request.UserName.Trim();
            var userNameLower = userName.ToLowerInvariant();

            var admin = await db.Admins.FirstOrDefaultAsync(a =>
            a.UserName == userNameLower);

            if(admin is null || !hasher.VerifyPassword(admin.PasswordHash, request.Password))
            {
                return Problem(
                title: "Unauthorized",
                detail: "Invalid UserName or password.",
                statusCode: StatusCodes.Status401Unauthorized);
            }
            Entity.Admin admin1 = admin;
            string accessToken = jwt.GenerateAccessToken(admin.UserName,"Admin");
            string refreshToken = jwt.GenerateRefreshToken();

            admin.RefreshToken = refreshToken;
            admin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await db.SaveChangesAsync();

            var resopnse = new AdminLoginResponse(accessToken, refreshToken);

            return Ok(resopnse);
        }
        [HttpPost("Refresh")]
        [AllowAnonymous]
        [EndpointSummary("Refresh Token")]
        [EndpointDescription("Validate refresh token and return new tokens.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AdminLoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AdminLoginResponse>> RefreshToken(
            [FromBody] AdminLoginRequest request,
            [FromServices] AppDbContext db,
            [FromServices] JwtProvider jwt)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return Problem(
                    title: "Bad Request",
                    detail: "Refresh Token is required.",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            var admin = await db.Admins.FirstOrDefaultAsync(a =>
             a.RefreshToken == request.RefreshToken);

            if (admin == null || admin.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                return Problem(
                    title: "Unauthorized",
                    detail: "Invalid or expired refresh token.",
                    statusCode: StatusCodes.Status401Unauthorized
                );
            }
            string newAccessToken = jwt.GenerateAccessToken(admin.Id.ToString(), admin.UserName);
            string newRefreshToken = jwt.GenerateRefreshToken();

            admin.RefreshToken = newRefreshToken;
            admin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await db.SaveChangesAsync();

            var response = new AdminLoginResponse(newAccessToken, newRefreshToken);
            return Ok(response);
        }
    }
}
