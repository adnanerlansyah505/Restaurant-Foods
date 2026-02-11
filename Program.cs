using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Data;
using RestaurantFoods.Repositories;
using RestaurantFoods.Repositories.Interfaces;
using RestaurantFoods.Services;
using RestaurantFoods.Services.Interfaces;
using RestaurantFoods.Services.Security;
using RestaurantFoods.Data.Seeders;
using RestaurantFoods.Utilities.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add Response Handlers
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value!.Errors.Count > 0)
            .ToDictionary(
                x => x.Key,
                x => x.Value!.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList()
            );

        return new BadRequestObjectResult(
            new ResponseHandlers<object>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Validation failed",
                Errors = errors
            }
        );
    };
});

// Add Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<PasswordHasher>();

// Add controllers
builder.Services.AddControllers();

// Add DbContext (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger (optional but recommended)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add Seeders
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var passwordHasher = services.GetRequiredService<PasswordHasher>();

    await RoleSeeder.SeedAsync(context);
    await UserSeeder.SeedAsync(context, passwordHasher);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
