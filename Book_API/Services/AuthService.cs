using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Book_API.Services.Interfaces;

using Microsoft.Extensions.Configuration;

namespace Book_API.Services
{
  public class AuthService : IAuthService
  {
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    /// <inheritdoc />
    public string GetSignInUrl()
    {
      var clientId = _configuration["AWS:UserPoolClientId"];
      var redirectUri = _configuration["AWS:RedirectUri"];
      var domain = _configuration["AWS:UserPoolDomain"];
      var responseType = _configuration["AWS:ResponseType"];
      var region = _configuration["AWS:Region"];
      // return cognito hosted ui url
      return $"https://{domain}.auth.{region}.amazoncognito.com/login?client_id={clientId}&response_type={responseType}&scope=email&redirect_uri={redirectUri}";
    }

    /// <inheritdoc />
    public async Task<(string accessToken, string refreshToken, bool isSuccess)> ExchangeTokens(string code)
    {
      if (string.IsNullOrEmpty(code))
      {
        Console.Error.WriteLine("Missing authorization code.");
        return (string.Empty, string.Empty, false);
      }

      var tokenResponse = await ExchangeCodeForTokensAsync(code);
      if (tokenResponse == null)
      {
        Console.Error.WriteLine("Error exchanging code for tokens.");
        return (string.Empty, string.Empty, false);
      }

      var accessToken = tokenResponse.RootElement.GetProperty("access_token").ToString();
      var refreshToken = tokenResponse.RootElement.GetProperty("refresh_token").ToString();

      return (accessToken, refreshToken, true);
    }


    private async Task<JsonDocument> ExchangeCodeForTokensAsync(string code)
    {
      using var httpClient = new HttpClient();
      var clientId = _configuration["AWS:UserPoolClientId"];
      var clientSecret = _configuration["AWS:UserPoolClientSecret"];
      var redirectUri = _configuration["AWS:RedirectUri"];
      var tokenEndpoint = $"https://{_configuration["AWS:UserPoolDomain"]}.auth.{_configuration["AWS:Region"]}.amazoncognito.com/oauth2/token";

      var content = new FormUrlEncodedContent(
      [
        new KeyValuePair<string, string>("grant_type", "authorization_code"),
        new KeyValuePair<string, string>("client_id", clientId),
        new KeyValuePair<string, string>("code", code),
        new KeyValuePair<string, string>("redirect_uri", redirectUri)
      ]);

      var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
      httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeaderValue);

      var response = await httpClient.PostAsync(tokenEndpoint, content);
      var responseContent = await response.Content.ReadAsStringAsync();

      if (response.IsSuccessStatusCode)
      {
        return JsonDocument.Parse(responseContent);
      }

      return null;
    }
  }

}
