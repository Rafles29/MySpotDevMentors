using MySpot.Application;
using MySpot.Core;
using MySpot.Infrastructure;
using MySpot.Infrastructure.Exceptions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddControllers();

builder.Host.UseSerilog(((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .WriteTo.File("logs/log.txt")
        .WriteTo.Seq("http://localhost:5341");
}));

var app = builder.Build();
app.UseInfrastructure();

app.Run();