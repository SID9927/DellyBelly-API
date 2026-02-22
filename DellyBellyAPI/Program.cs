using DellyBelly.Application.Interfaces;
using DellyBelly.API.Middlewares;
using DellyBelly.Application.Services;
using DellyBelly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext with connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IGalleryService, GalleryService>();

// Optimization: Add Memory Cache
builder.Services.AddMemoryCache();

// Optimization: Add Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Optimization: Add Rate Limiting for "crowded site" scenarios
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 1000, // 1000 requests
                QueueLimit = 100,
                Window = TimeSpan.FromMinutes(1) // per minute
            }));
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();        // Enables Swagger JSON
    app.UseSwaggerUI();      // Enables Swagger UI
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Optimization: Use Response Compression
app.UseResponseCompression();

// Register Logging Middleware (Must be AFTER ResponseCompression to capture uncompressed body)
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// Optimization: Use Rate Limiting
app.UseRateLimiter();

app.UseAuthorization();


app.MapControllers();

app.Run();