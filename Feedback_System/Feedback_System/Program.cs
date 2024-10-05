using FluentValidation.AspNetCore;
using Feedback_System.Dtos;
using Feedback_System.Abstractions;
using Feedback_System.Concretes;
using Microsoft.EntityFrameworkCore;
using Feedback_System.Validations;  // Validator sınıflarını kullanmak için ekliyoruz

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
// FluentValidation'ı entegre ediyoruz
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ParkDtoValidator>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with SQL Server connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories for Dependency Injection
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
builder.Services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
