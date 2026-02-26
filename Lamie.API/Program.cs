using Lamie.API.Middlewares;
using Lamie.Application;
using Lamie.Application.Common.Interfaces;
using Lamie.Application.Users;
using Lamie.Domain.Repositories;
using Lamie.Infrastructure.Persistence;
using Lamie.Infrastructure.Persistence.Repositories;
using Lamie.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly)
);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Default"),
        x => x.MigrationsAssembly("Infrastructure")
    );

    options.UseSnakeCaseNamingConvention();
});

// Repository
builder.Services.AddScoped<ISysUserRepository, SysUserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
