using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using Book_API.Services.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Book_API.Services
{
  public class JwtTokenService : IJwtTokenService
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtTokenService(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    public Guid GetUserId()
    {
      var payload = GetPayload();
      if (payload.TryGetValue("sub", out object claimValue))
      {
        return Guid.Parse(claimValue.ToString());
      }
      return default;
    }

    private IDictionary<string, object> GetPayload()
    {
      var context = _httpContextAccessor.HttpContext;
      if (context.Request.Cookies.TryGetValue("access_token", out string token))
      {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken.Payload;
      }
      return default;
    }
  }
}
