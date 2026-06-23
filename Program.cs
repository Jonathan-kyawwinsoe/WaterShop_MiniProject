using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using water_shop.Data;
using water_shop.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi(option =>
{
    option.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {

            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme."
        };
        document.Tags ??= new HashSet<OpenApiTag>();
        return Task.CompletedTask;
    });
    option.AddOperationTransformer((operation, context, cancellationToken) =>
    {
        if (context.Description.ActionDescriptor.EndpointMetadata.Any(m => m is AuthorizeAttribute))
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                { new OpenApiSecuritySchemeReference("Bearer"), new List<string>() }
            });
        }
        return Task.CompletedTask;
    });
});
var connectingString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectingString));
builder.Services.Configure<JwtOption>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference(option =>
{
    option.Authentication = new ScalarAuthenticationOptions
    {
        PreferredSecuritySchemes = ["Scheme"]
    };
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
