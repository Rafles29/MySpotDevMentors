using MySpot.Api.Repositories;
using MySpot.Application;
using MySpot.Application.Services;
using MySpot.Core;
using MySpot.Infrastructure;
using MySpot.Infrastructure.Time;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();