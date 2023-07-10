using Microsoft.AspNetCore.Http;
using MySpot.Application.DTO;
using MySpot.Application.Security;

namespace MySpot.Infrastructure.Auth;

internal sealed class HttpContextTokenStorage : ITokenStorage
{
    private const string JwtKey = "jwt";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTokenStorage(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Set(JwtDto jwt) => _httpContextAccessor.HttpContext?.Items.TryAdd(JwtKey, jwt);

    public JwtDto? Get()
    {
        if(_httpContextAccessor.HttpContext is null)
        {
            return null;
        }
        if(_httpContextAccessor.HttpContext.Items.TryGetValue(JwtKey, out var jwt))
        {
            return jwt as JwtDto;
        }

        return null;
    }
}