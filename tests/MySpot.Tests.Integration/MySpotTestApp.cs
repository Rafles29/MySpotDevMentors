using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MySpot.Tests.Integration;

internal sealed class MySpotTestApp : WebApplicationFactory<Program>
{
    public HttpClient Client { get; private set; } = null!;

    public MySpotTestApp()
    {
        Client = WithWebHostBuilder(builder => { builder.UseEnvironment("test"); }).CreateClient();
    }
}