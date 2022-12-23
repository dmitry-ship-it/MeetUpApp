using MeetUpApp.Api.Middleware;
using MeetUpApp.Api.Middleware.Extensions;
using MeetUpApp.Data.Extensions;
using MeetUpApp.Managers.Extensions;
using MeetUpApp.ViewModels.Mapping;
using MeetUpApp.ViewModels.Validation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog();

builder.Services.AddDbContextWithRepositories(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MeetupProfile));
builder.Services.AddControllers();
builder.Services.AddPreconfiguredFluentValidation();
builder.Services.AddManagers();
builder.Services.AddJwtBearerAuthentication(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.TryAddFirstUser();
app.AddSwaggerWithUI(app.Environment);
app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseSessionWithJwtBearer();
app.AddAuthenticationAndAuthorization();
app.MapControllers();
app.Run();