using Example.SocialFeed.Api.Middleware;
using Example.SocialFeed.Application.Interfaces;
using Example.SocialFeed.Application.Services;
using Example.SocialFeed.Infrastructure.Identity;
using Example.SocialFeed.Infrastructure.Persistence;
using Example.SocialFeed.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string AngularDevClientCorsPolicy = "AngularDevClient";

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Example Social Feed API",
        Version = "v1",
        Description = "Backend API for the IT 08-1 post & comments page."
    });
});

// EF Core - SQLite file database. Connection string can be overridden via appsettings/env.
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? "Data Source=socialfeed.db";
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

// Repositories
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

// Application services
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();

// Identity (stubbed - no login flow in this exercise)
builder.Services.AddSingleton<ICurrentUserProvider, FixedCurrentUserProvider>();

// CORS - allow the local Angular dev server to call the API.
var angularDevOrigin = builder.Configuration["AngularDevClientOrigin"] ?? "http://localhost:4200";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularDevClientCorsPolicy, policy =>
    {
        policy.WithOrigins(angularDevOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Ensure the database exists and seed data is applied (no migrations needed for this exercise).
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandlingMiddleware();

app.UseHttpsRedirection();

app.UseCors(AngularDevClientCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();

// Exposed for WebApplicationFactory-based integration tests.
public partial class Program
{
}
