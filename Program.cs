using SkillManagement.Api.Data;
using FluentValidation.AspNetCore;
using System.Reflection;
using FluentValidation;
using SkillManagement.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add FluentValidation and register all validators in the assembly
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddFluentValidationAutoValidation(); 


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISkillManagementRepository, SkillManagementRepository>();

// Register the service for business logic
builder.Services.AddScoped<ISkillManagementService, SkillManagementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map the routes from the controllers.
app.MapControllers();

app.Run();