using FluentValidation;
using FluentValidation.AspNetCore;
using MeetUpApp.Api.Middleware;
using MeetUpApp.Api.Middleware.Extensions;
using MeetUpApp.Data;
using MeetUpApp.Data.DAL;
using MeetUpApp.Data.Models;
using MeetUpApp.Managers;
using MeetUpApp.ViewModels.Mapping;
using MeetUpApp.ViewModels.Validation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog();

// Add services to the container.
builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultDb")));

builder.Services.AddAutoMapper(
    typeof(Program), typeof(MeetupProfile));

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserModelValidator>();

// DI
builder.Services.AddScoped<IRepository<Meetup>, MeetupRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<MeetupManager>();

// authentication
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSessionForJwtBearer();
builder.Services.AddAuthenticationForJwtBearer()
    .AddPreconfiguredJwtBearer(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationRulesToSwagger();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();

// remove if at least one user is already exists
// !! use this only for testing
app.TryAddFirstUser();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookiePolicy();
app.UseSession();
app.UseJwtBearer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();