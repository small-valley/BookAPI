using System.Threading.Tasks;

namespace Book_API.Services.Interfaces
{
  public interface IAuthService
  {
    /// <summary>
    /// Get the signin URL
    /// </summary>
    /// <returns></returns>
    string GetSignInUrl();

    /// <summary>
    /// Exchange the authorization code for tokens
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<(string accessToken, string refreshToken, bool isSuccess)> ExchangeTokens(string code);
  }
}
