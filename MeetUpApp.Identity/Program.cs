using MeetUpApp.Identity.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);
builder.Services.AddPreconfiguredIdentityServer(builder.Configuration);

var app = builder.Build();

app.UpdateIdentityDbTables(app.Configuration);
app.UseIdentityServer();

app.Run();
