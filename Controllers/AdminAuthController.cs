using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
            string accessToken = jwt.GenerateAccessToken(admin.Id.ToString(),admin.UserName);
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
        [ProducesResponseType(typeof(AdminTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AdminTokenResponse>> RefreshToken(
            [FromBody] AdminTokenRequest request,
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
        [HttpPost("Change_Password")]
        [Authorize]
        [EndpointSummary("Change Password")]
        [EndpointDescription("Change the Current Admin Password")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AdminChangePasswordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AdminChangePasswordResponse>> ChangePassword(
            [FromBody] AdminChangePasswordRequest request,
            [FromServices] AppDbContext db,
            [FromServices] PasswordHasher hasher)
        {
            if(request.NewPassword != request.ConfirmPassword)
            {
                return Problem(
                    title: "Validation Error",
                    detail: "New password and Confirm Password do not match",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            var adminIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(adminIdClaim is null)
            {
                return Problem(

                    title: "Unauthorized",
                    detail: "Invalid Token",
                    statusCode: StatusCodes.Status401Unauthorized
                );
            }
            int adminId = int.Parse(adminIdClaim);
            var admin = await db.Admins.FindAsync(adminId);
            if(admin is null)
            {
                return Problem(
                    title: "Not Found",
                    detail: "Admin account not found",
                    statusCode: StatusCodes.Status404NotFound
                
                );
            }
            if (!hasher.VerifyPassword(admin.PasswordHash, request.CurrentPassword))
            {
                return Problem(
                    title: "Validation Error",
                    detail: "Current password is incorrect",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            admin.PasswordHash = hasher.HashPassword(request.NewPassword);
            await db.SaveChangesAsync();
            return Ok(new AdminChangePasswordResponse(Message: "Password changed successfully. Please login again"));
        }
        [HttpPost("Logout")]
        [AllowAnonymous]
        [EndpointSummary("Logout")]
        [EndpointDescription("Admin Logout use refreshToken")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AdminLoginResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AdminLogoutResponse>> Logout(
            [FromBody] AdminLogoutRequest request,
            [FromServices] AppDbContext db,
            [FromServices] JwtProvider jwt
            )
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return Problem(
                title: "Invalid request",
                detail: "Refresh token is required.",
                statusCode: StatusCodes.Status400BadRequest);
            }
            var admin = await db.Admins.FirstOrDefaultAsync(a => a.RefreshToken == request.RefreshToken);
            if(admin == null)
            {
                return Problem(
                    title: "Unauthorized",
                    detail: "Invalid refresh token",
                    statusCode: StatusCodes.Status400BadRequest);
            }
            admin.RefreshToken = null;
            admin.RefreshTokenExpiry = null;

            await db.SaveChangesAsync();

            return Ok(new AdminLogoutResponse(Message: "Logout successfully"));
        }
    }
}