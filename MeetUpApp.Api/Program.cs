using MeetUpApp.Api.Middleware;
using MeetUpApp.Api.Middleware.Extensions;
using MeetUpApp.Data.Extensions;
using MeetUpApp.Managers.Extensions;
using MeetUpApp.ViewModels.Mapping;
using MeetUpApp.ViewModels.Validation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);

builder.Services.AddDbContextWithRepositories(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MeetupProfile));
builder.Services.AddControllers();
builder.Services.AddPreconfiguredFluentValidation();
builder.Services.AddManagers();
builder.Services.AddSwaggerGenWithOAuth(builder.Configuration);
builder.Services.AddJwtBearerAuthentication(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.UseSwaggerWithUI();
app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseSession();
app.UseAuthenticationAndAuthorization();
app.MapControllers();
app.Run();