using MeetUpApp.Identity.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPreconfiguredIdentityServer(builder.Configuration);

var app = builder.Build();

app.UseIdentityServer();

app.Run();
