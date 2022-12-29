namespace MultiPurposeProject.Authorization;

using Microsoft.AspNetCore.Mvc;
using MultiPurposeProject.Helpers;
using MultiPurposeProject.Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var userId = jwtUtils.ValidateToken(token);

        if (userId != null)
        {
            try
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userService.GetById(userId.Value);
            }catch(Exception)
            {
                context.Items["User"] = null;
            }
        }

        await _next(context);
    }
}