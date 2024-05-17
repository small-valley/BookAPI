using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
namespace Book_API.Middlewares
{
  public class TokenMiddleware
  {
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      // TODO: Refresh token if needed

      // Append the access token to the Authorization header
      var accessToken = context.Request.Cookies["access_token"];
      if (!string.IsNullOrEmpty(accessToken))
      {
        context.Request.Headers.Append("Authorization", $"Bearer {accessToken}");
      }

      await _next(context);
    }
  }
}
