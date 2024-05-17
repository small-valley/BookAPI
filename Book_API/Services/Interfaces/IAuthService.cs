using System.Threading.Tasks;

namespace Book_API.Services.Interfaces
{
  public interface IAuthService
  {
    /// <summary>
    /// Exchange the authorization code for tokens
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<(string accessToken, string refreshToken, bool isSuccess)> ExchangeTokens(string code);
  }
}
