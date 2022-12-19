using MeetUpApp.Api;
using MeetUpApp.Data;
using MeetUpApp.Data.DAL;
using MeetUpApp.Data.Models;
using MeetUpApp.Managers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDb")));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();

// DI
builder.Services.AddScoped<IRepository<Meetup>, MeetupRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<MeetupManager>();

// authentication
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSessionForJwtBearer();
builder.Services.AddAuthenticationForJwtBearer()
    .AddPreconfiguredJwtBearer(
        builder.Configuration.GetSection("AuthSettings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCustomExceptionHandler();

// remove if at least one user is already exists
// !! use this only for testing 
app.Services.TryAddFirstUser();

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
