using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using APICatalogo.Services;


namespace APICatalogo.Middlewares;

public class TokenRevocationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenRevocationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ITokenRevocationService tokenRevocationService)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token) && tokenRevocationService.IsTokenRevoked(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("O token foi revogado...");
            return;
        }

        await _next(context);
    }
}
