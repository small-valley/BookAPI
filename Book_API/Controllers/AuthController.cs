using System;
using System.Threading.Tasks;

using Book_API.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Book_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration, IAuthService authService)
    {
      _configuration = configuration;
      _authService = authService;
    }
    [HttpGet("signin")]

    public IActionResult SignIn()
    {
      var url = _authService.GetSignInUrl();
      return new JsonResult(url);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback(string code)
    {
      var result = await _authService.ExchangeTokens(code);

      if (result.isSuccess == false)
      {
        return Redirect(_configuration["Frontend:AuthFailRedirectUri"]);
      }

      // Set the tokens in cookies
      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None,
        Expires = DateTime.UtcNow.AddDays(1)
      };

      Response.Cookies.Append("access_token", result.accessToken, cookieOptions);
      Response.Cookies.Append("refresh_token", result.refreshToken, cookieOptions);

      // Redirect to the front-end home page
      return Redirect(_configuration["Frontend:SigninSuccessRedirectUri"]);
    }
  }
}
